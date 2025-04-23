using ECommerceRESTfulAPI.Repositories.Interfaces;
using EntityFrameworkCore.MySQL.Data;
using EntityFrameworkCore.MySQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRESTfulAPI.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ToListAsync();

        public async Task<Order?> GetByIdAsync(int id) =>
            await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
