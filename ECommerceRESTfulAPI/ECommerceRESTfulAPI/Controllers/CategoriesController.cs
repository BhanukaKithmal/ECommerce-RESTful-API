using ECommerceRESTfulAPI.DTOs.Category;
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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoriesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // POST: API/Categories
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _appDbContext.Categories.Add(category);
            await _appDbContext.SaveChangesAsync();

            return Ok(category);
        }


        // GET: API/Categories
        [HttpGet]
        public async Task<ActionResult<PagedResponse<CategoryDto>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var query = _appDbContext.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            var response = new PagedResponse<CategoryDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = categories
            };

            return Ok(response);
        }

        // GET: API/Categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        // PUT: API/Categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto dto)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _appDbContext.SaveChangesAsync();
            return Ok(category);
        }

        // DELETE: API/Categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
