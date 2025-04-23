using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.MySQL.Models;
using EntityFrameworkCore.MySQL.Repositories.Interfaces;
using EntityFrameworkCore.MySQL.Services;
using EntityFrameworkCore.MySQL.Services.Interfaces;
using ECommerceRESTfulAPI.DTOs.Category;

namespace EntityFrameworkCore.MySQL.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _service = new CategoryService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, Name = "Electronics", Description = "Devices" },
                new Category { CategoryId = 2, Name = "Books", Description = "Reading materials" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            var list = result.ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal("Electronics", list[0].Name);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCategoryDto()
        {
            // Arrange
            var category = new Category { CategoryId = 1, Name = "Sports", Description = "Outdoor games" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result?.CategoryId);
            Assert.Equal("Sports", result?.Name);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Category?)null);

            var result = await _service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsAndReturnsCreatedCategory()
        {
            // Arrange
            var input = new CategoryDto { Name = "Gaming", Description = "Consoles and accessories" };
            var created = new Category { CategoryId = 1, Name = input.Name, Description = input.Description };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Category>())).ReturnsAsync(created);

            // Act
            var result = await _service.AddAsync(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Gaming", result.Name);
            Assert.Equal(1, result.CategoryId);
        }

        [Fact]
        public async Task UpdateAsync_ValidUpdate_ReturnsUpdatedCategory()
        {
            var input = new CategoryDto { CategoryId = 1, Name = "Updated", Description = "Updated description" };
            var updated = new Category { CategoryId = 1, Name = "Updated", Description = "Updated description" };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(updated);

            var result = await _service.UpdateAsync(input);

            Assert.NotNull(result);
            Assert.Equal("Updated", result?.Name);
        }

        [Fact]
        public async Task UpdateAsync_NullReturnedFromRepo_ReturnsNull()
        {
            var input = new CategoryDto { CategoryId = 99, Name = "NonExistent", Description = "Does not exist" };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Category>())).ReturnsAsync((Category?)null);

            var result = await _service.UpdateAsync(input);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepoAndReturnsTrue()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalseWhenNotFound()
        {
            _mockRepo.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(999);

            Assert.False(result);
        }
    }
}
