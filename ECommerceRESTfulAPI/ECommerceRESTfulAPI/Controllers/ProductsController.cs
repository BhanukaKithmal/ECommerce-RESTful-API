using ECommerceRESTfulAPI.DTOs.Product;
using ECommerceRESTfulAPI.Enum;
using EntityFrameworkCore.MySQL.Data;
using EntityFrameworkCore.MySQL.DTOs;
using EntityFrameworkCore.MySQL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.MySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ProductsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // POST: API/Products
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto dto)
        {
            // Ensure the category is valid
            if (!Enum.IsDefined(typeof(CategoryType), dto.Category))
            {
                return BadRequest("Invalid category.");
            }

            // Create the product entity from the DTO
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category  // Use the enum value for Category
            };

            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();

            return Ok(product);
        }

        // GET: API/Products
        [HttpGet]
        public async Task<ActionResult<PagedResponse<ProductDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] CategoryType? category = null)
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) || p.Description.Contains(search));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.Category == category);
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    Category = p.Category,
                })
                .ToListAsync();

            var response = new PagedResponse<ProductDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = products
            };

            return Ok(response);
        }

        // GET: API/Products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // PUT: API/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductCreateDto dto)
        {
            var existingProduct = await _appDbContext.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound("Product not found.");

            // Ensure the category is valid
            if (!Enum.IsDefined(typeof(CategoryType), dto.Category))
            {
                return BadRequest("Invalid category.");
            }

            // Update fields from the DTO
            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;
            existingProduct.Category = dto.Category;

            await _appDbContext.SaveChangesAsync();
            return Ok(existingProduct);
        }

        // DELETE: API/Products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
