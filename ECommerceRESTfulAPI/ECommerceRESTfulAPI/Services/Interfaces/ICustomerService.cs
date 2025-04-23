using EntityFrameworkCore.MySQL.DTOs;
using EntityFrameworkCore.MySQL.Models;

namespace EntityFrameworkCore.MySQL.Services
{
    public interface ICustomerService
    {
        Task<PagedResponse<CustomerDto>> GetPagedCustomers(int pageNumber, int pageSize, string? search);
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
    }
}
