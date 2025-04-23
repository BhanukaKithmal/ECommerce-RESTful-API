// Services/CategoryService.cs
using ECommerceRESTfulAPI.DTOs.Category;
using EntityFrameworkCore.MySQL.Models;
using EntityFrameworkCore.MySQL.Repositories.Interfaces;
using EntityFrameworkCore.MySQL.Services.Interfaces;

namespace EntityFrameworkCore.MySQL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryDto> AddAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            var created = await _repository.AddAsync(category);

            return new CategoryDto
            {
                CategoryId = created.CategoryId,
                Name = created.Name,
                Description = created.Description
            };
        }

        public async Task<CategoryDto?> UpdateAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                CategoryId = categoryDto.CategoryId,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            var updated = await _repository.UpdateAsync(category);
            if (updated == null) return null;

            return new CategoryDto
            {
                CategoryId = updated.CategoryId,
                Name = updated.Name,
                Description = updated.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
