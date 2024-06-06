using CustomerManagementSystem.Models;
using CustomerManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            await _orderService.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.OrderId) return BadRequest();
            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
