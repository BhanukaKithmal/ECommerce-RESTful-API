// Services/Interfaces/ICategoryService.cs
using ECommerceRESTfulAPI.DTOs.Category;

namespace EntityFrameworkCore.MySQL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> AddAsync(CategoryDto categoryDto);
        Task<CategoryDto?> UpdateAsync(CategoryDto categoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
