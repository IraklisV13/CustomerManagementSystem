using Microsoft.AspNetCore.SignalR;

namespace CustomerManagementSystem.Notifications
{
    public class OrderHub : Hub
    {
        public async Task SendOrderNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveOrderNotification", message);
        }
    }
}
