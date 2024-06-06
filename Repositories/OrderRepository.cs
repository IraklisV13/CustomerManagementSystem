using CustomerManagementSystem.DBContexts;
using CustomerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Repositories
{
    public class OrderRepository : IOrderRepository
        {
            private readonly CMSContext _context;

            public OrderRepository(CMSContext context)
            {
                _context = context;
            }

            public async Task<Order> GetOrderByIdAsync(int orderId)
            {
                return await _context.Orders.FindAsync(orderId);
            }

            public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            {
                return await _context.Orders.ToListAsync();
            }

            public async Task AddOrderAsync(Order order)
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateOrderAsync(Order order)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteOrderAsync(int orderId)
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
