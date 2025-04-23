using EntityFrameworkCore.MySQL.DTOs;
using EntityFrameworkCore.MySQL.Models;
using EntityFrameworkCore.MySQL.Repositories;

namespace EntityFrameworkCore.MySQL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponse<CustomerDto>> GetPagedCustomers(int pageNumber, int pageSize, string? search)
        {
            var allCustomers = await _repository.GetAllAsync();
            var filtered = allCustomers.Where(c => string.IsNullOrEmpty(search) ||
                                                  (c.FirstName + " " + c.LastName).ToLower().Contains(search.ToLower()));

            var total = filtered.Count();
            var paged = filtered
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    FullName = $"{c.FirstName} {c.LastName}",
                    Email = c.Email
                }).ToList();

            return new PagedResponse<CustomerDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Items = paged
            };
        }

        public async Task<Customer?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(Customer customer) => await _repository.AddAsync(customer);
        public async Task UpdateAsync(Customer customer) => await _repository.UpdateAsync(customer);
        public async Task DeleteAsync(Customer customer) => await _repository.DeleteAsync(customer);
    }
}
