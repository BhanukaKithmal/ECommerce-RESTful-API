using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.MySQL.DTOs;
using EntityFrameworkCore.MySQL.Models;
using EntityFrameworkCore.MySQL.Repositories;
using EntityFrameworkCore.MySQL.Services;

namespace ECommerceRESTfulAPI.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _service = new CustomerService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetPagedCustomers_ReturnsPagedResponse()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" },
                new Customer { CustomerId = 3, FirstName = "Jim", LastName = "Beam", Email = "jim@example.com" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _service.GetPagedCustomers(pageNumber: 1, pageSize: 2, search: "Jane");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal("Jane Smith", result.Items.First().FullName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectCustomer()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email="john@gmail.com" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAdd()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FirstName = "Test", LastName = "User", Email="example@gmail.com" };

            // Act
            await _service.AddAsync(customer);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(customer), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FirstName = "Test", LastName = "User", Email = "example@gmail.com" };

            // Act
            await _service.UpdateAsync(customer);

            // Assert
            _mockRepo.Verify(r => r.UpdateAsync(customer), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDelete()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FirstName = "Test", LastName = "User", Email = "example@gmail.com" };

            // Act
            await _service.DeleteAsync(customer);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(customer), Times.Once);
        }
    }
}
