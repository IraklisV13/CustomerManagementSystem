using CustomerManagementSystem.Models;
using CustomerManagementSystem.Repositories;

namespace CustomerManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            var user = await _userRepository.GetUserByIdAsync(order.UserId);
            if (user.UserType == UserType.Mobile)
            {
                // Logic specific to mobile users
            }
            else if (user.UserType == UserType.Web)
            {
                // Logic specific to web users
            }

            await _orderRepository.AddOrderAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
