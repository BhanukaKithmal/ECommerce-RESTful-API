using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceRESTfulAPI.Repositories.Interfaces;
using ECommerceRESTfulAPI.Services.Implementations;
using ECommerceRESTfulAPI.Services.Interfaces;
using EntityFrameworkCore.MySQL.Models;

namespace ECommerceRESTfulAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly IProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Item1", Price = 100 },
                new Product { ProductId = 2, Name = "Item2", Price = 200 }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.LongCount());
        }
    }
}
