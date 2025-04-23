using ECommerceRESTfulAPI.DTOs;
using ECommerceRESTfulAPI.DTOs.Order;
using EntityFrameworkCore.MySQL.Data;
using EntityFrameworkCore.MySQL.DTOs;
using EntityFrameworkCore.MySQL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.MySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public OrdersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // POST: API/Orders
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderCreateDto orderDto)
        {
            if (orderDto.CustomerId == 0 || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            {
                return BadRequest("Invalid data.");
            }

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            // Process the order items
            foreach (var item in orderDto.OrderItems)
            {
                if (item.ProductId == 0 || item.Quantity == 0)
                {
                    return BadRequest("Invalid product ID or quantity.");
                }

                var product = await _appDbContext.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with ID {item.ProductId} not found.");
                }

                var unitPrice = product.Price;
                var subtotal = unitPrice * item.Quantity;

                totalAmount += subtotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice
                });
            }

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderItems = orderItems,
                TotalAmount = totalAmount, // Calculate total amount from items
                OrderDate = DateTime.Now
            };

            _appDbContext.Orders.Add(order);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        // GET: API/Orders
        [HttpGet]
        public async Task<ActionResult<PagedResponse<OrderDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? customerName = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = _appDbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                query = query.Where(o =>
                    o.Customer.FirstName.Contains(customerName) ||
                    o.Customer.LastName.Contains(customerName));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= toDate.Value);
            }

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            var response = new PagedResponse<OrderDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = orders
            };

            return Ok(response);
        }

        // GET: API/Orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _appDbContext.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return Ok(new OrderDto
            {
                OrderId = order.OrderId,
                CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
            });
        }

        // PUT: API/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto orderDto)
        {
            if (orderDto.CustomerId == 0 || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            {
                return BadRequest("Invalid data.");
            }

            var order = await _appDbContext.Orders
                .Include(o => o.OrderItems) // Include OrderItems for update
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound("Order not found.");
            if (id != order.OrderId) return BadRequest("Order ID mismatch.");

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            // Process the order items and update them
            foreach (var item in orderDto.OrderItems)
            {
                if (item.ProductId == 0 || item.Quantity == 0)
                {
                    return BadRequest("Invalid product ID or quantity.");
                }

                var product = await _appDbContext.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with ID {item.ProductId} not found.");
                }

                var unitPrice = product.Price;
                var subtotal = unitPrice * item.Quantity;

                totalAmount += subtotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice
                });
            }

            // Update the order with the new details
            order.CustomerId = orderDto.CustomerId;
            order.OrderItems = orderItems;
            order.TotalAmount = totalAmount; // Auto-calculate total amount based on items

            await _appDbContext.SaveChangesAsync();
            return Ok(order);
        }

        // DELETE: API/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _appDbContext.Orders.FindAsync(id);
            if (order == null)
                return NotFound("Order not found.");

            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
