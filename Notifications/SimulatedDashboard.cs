using CustomerManagementSystem.DBContexts;
using CustomerManagementSystem.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace CustomerManagementSystem.Notifications
{
    public class SimulatedDashboard
    {
        private readonly HubConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public SimulatedDashboard(string hubUrl, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _connection.On<string>("ReceiveOrderNotification", async (message) =>
            {
                Console.WriteLine($"Dashboard Notification: {message}");
                await LogNotificationToDatabase(message);
            });
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
            Console.WriteLine("Simulated Dashboard connected. Listening for notifications...");
        }

        public async Task StopAsync()
        {
            await _connection.StopAsync();
            Console.WriteLine("Simulated Dashboard disconnected.");
        }

        private async Task LogNotificationToDatabase(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CMSContext>();
                var notification = new Notification
                {
                    Message = message,
                    Timestamp = DateTime.Now
                };
                context.Notifications.Add(notification);
                await context.SaveChangesAsync();
            }
        }
    }
}
