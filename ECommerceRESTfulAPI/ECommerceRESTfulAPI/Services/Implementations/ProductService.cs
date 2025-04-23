using ECommerceRESTfulAPI.Repositories.Interfaces;
using ECommerceRESTfulAPI.Services.Interfaces;
using EntityFrameworkCore.MySQL.Models;

namespace ECommerceRESTfulAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _productRepo.GetAllAsync();

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await _productRepo.GetByIdAsync(id);

        public async Task CreateProductAsync(Product product) =>
            await _productRepo.AddAsync(product);

        public async Task UpdateProductAsync(Product product) =>
            await _productRepo.UpdateAsync(product);

        public async Task DeleteProductAsync(int id) =>
            await _productRepo.DeleteAsync(id);
    }
}
