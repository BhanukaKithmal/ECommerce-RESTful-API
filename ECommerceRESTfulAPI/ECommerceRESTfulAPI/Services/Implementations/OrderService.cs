using ECommerceRESTfulAPI.Repositories.Interfaces;
using ECommerceRESTfulAPI.Services.Interfaces;
using EntityFrameworkCore.MySQL.Models;

namespace ECommerceRESTfulAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository orderRepo, IProductService productService)
        {
            _orderRepo = orderRepo;
            _productService = productService;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync() =>
            await _orderRepo.GetAllAsync();

        public async Task<Order?> GetOrderByIdAsync(int id) =>
            await _orderRepo.GetByIdAsync(id);

        public async Task CreateOrderAsync(Order order)
        {
            decimal totalAmount = 0;

            foreach (var item in order.OrderItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {item.ProductId} not found.");
                }

                // Optional: Validate product stock here (comment out if not needed)
                if (product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product ID {item.ProductId}.");
                }

                // Deduct stock
                product.Stock -= item.Quantity;
                await _productService.UpdateProductAsync(product);

                item.UnitPrice = product.Price;
                totalAmount += item.UnitPrice * item.Quantity;
            }

            order.TotalAmount = totalAmount;
            order.OrderDate = DateTime.UtcNow;

            await _orderRepo.AddAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            // Optional: recalculate total amount here as well, similar to CreateOrderAsync
            await _orderRepo.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id) =>
            await _orderRepo.DeleteAsync(id);
    }
}
