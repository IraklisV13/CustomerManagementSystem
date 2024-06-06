using CustomerManagementSystem.Models;

    namespace CustomerManagementSystem.Services
    {
        public interface IOrderService
        {
            Task<Order> GetOrderByIdAsync(int orderId);
            Task<IEnumerable<Order>> GetAllOrdersAsync();
            Task AddOrderAsync(Order order);
            Task UpdateOrderAsync(Order order);
            Task DeleteOrderAsync(int orderId);
        }
    }
