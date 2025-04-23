using ECommerceRESTfulAPI.DTOs.Customer;
using ECommerceRESTfulAPI.Helpers;
using EntityFrameworkCore.MySQL.Data;
using EntityFrameworkCore.MySQL.DTOs;
using EntityFrameworkCore.MySQL.Models;
using EntityFrameworkCore.MySQL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Xunit.Sdk;

namespace EntityFrameworkCore.MySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        

        public CustomersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // POST: API/Categories
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new Customer
            {
                FirstName = Utility.NormalizeWhitespace(dto.FirstName),
                LastName = Utility.NormalizeWhitespace(dto.LastName),
                Email = Utility.NormalizeWhitespace(dto.Email),
                Phone = dto.Phone
            };

            _appDbContext.Customers.Add(customer);
            await _appDbContext.SaveChangesAsync();
            return Ok(customer);
        }


        // GET: API/Categories
        [HttpGet]
        public async Task<ActionResult<PagedResponse<CustomerDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var query = _appDbContext.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .Select(c=> new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FirstName + " "+ c.LastName,
                    Email = c.Email
                }).ToListAsync();

            var response = new PagedResponse<CustomerDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = customers
            };
            return Ok(response);
        }

        // GET: API/Categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _appDbContext.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return Ok(new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = $"{customer.FirstName} {customer.LastName}",
                Email = customer.Email
            });
        }

        // PUT: API/Categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDto dto)
        {
            var customer = await _appDbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer not found.");

            var emailExists = await _appDbContext.Customers
                .AnyAsync(c => c.Email.ToLower() == dto.Email.ToLower() && c.CustomerId != id);

            if (emailExists)
            {
                return Conflict("Another customer with this email already exists.");
            }

            customer.FirstName = Utility.NormalizeWhitespace(dto.FirstName);
            customer.LastName = Utility.NormalizeWhitespace(dto.LastName);
            customer.Email = Utility.NormalizeWhitespace(dto.Email);
            customer.Phone = dto.Phone;

            await _appDbContext.SaveChangesAsync();
            return Ok(customer);
        }


        // DELETE: API/Categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _appDbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer not found.");

            _appDbContext.Customers.Remove(customer);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
