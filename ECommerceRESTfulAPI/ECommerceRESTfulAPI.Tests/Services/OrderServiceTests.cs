using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceRESTfulAPI.Repositories.Interfaces;
using ECommerceRESTfulAPI.Services.Implementations;
using ECommerceRESTfulAPI.Services.Interfaces;
using EntityFrameworkCore.MySQL.Models;

namespace ECommerceRESTfulAPI.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly Mock<IProductService> _mockProductService;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockProductService = new Mock<IProductService>();
            _service = new OrderService(_mockOrderRepo.Object, _mockProductService.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { OrderId = 1 }, new Order { OrderId = 2 } };
            _mockOrderRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await _service.GetAllOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Order>)result).Count);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsCorrectOrder()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _service.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result?.OrderId);
        }

        [Fact]
        public async Task CreateOrderAsync_ValidOrder_CalculatesTotalAndUpdatesStock()
        {
            // Arrange
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2 }
                }
            };

            var product = new Product { ProductId = 1, Price = 100, Stock = 10 };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);
            _mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _mockOrderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            await _service.CreateOrderAsync(order);

            // Assert
            Assert.Equal(200, order.TotalAmount); 

            Assert.Equal(8, product.Stock);
            _mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ThrowsIfProductNotFound()
        {
            // Arrange
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 999, Quantity = 1 }
                }
            };

            _mockProductService.Setup(s => s.GetProductByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateOrderAsync(order));
            Assert.Contains("Product with ID 999 not found", ex.Message);
        }

        [Fact]
        public async Task CreateOrderAsync_ThrowsIfInsufficientStock()
        {
            // Arrange
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 5 }
                }
            };

            var product = new Product { ProductId = 1, Stock = 2, Price = 100 };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateOrderAsync(order));
            Assert.Contains("Insufficient stock", ex.Message);
        }

        [Fact]
        public async Task UpdateOrderAsync_CallsRepoUpdate()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderRepo.Setup(r => r.UpdateAsync(order)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateOrderAsync(order);

            // Assert
            _mockOrderRepo.Verify(r => r.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_CallsRepoDelete()
        {
            // Arrange
            _mockOrderRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteOrderAsync(1);

            // Assert
            _mockOrderRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
