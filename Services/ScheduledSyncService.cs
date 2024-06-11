namespace CustomerManagementSystem.Services
{
    public class ScheduledSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduledSyncService> _logger;
        private Timer _timer;

        public ScheduledSyncService(IServiceProvider serviceProvider, ILogger<ScheduledSyncService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Calculate the initial delay to start the timer at 3 AM UTC every day
            var now = DateTime.UtcNow;
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 3, 0, 0, DateTimeKind.Utc);
            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }
            var initialDelay = nextRunTime - now;

            _timer = new Timer(SyncData, null, initialDelay, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void SyncData(object state)
        {
            _logger.LogInformation("ScheduledSyncService is working.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var syncService = scope.ServiceProvider.GetRequiredService<SynchronizationService>();
                syncService.SyncProductsAsync(0).GetAwaiter().GetResult();
            }
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
