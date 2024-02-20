using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pdsr.Hosting
{
    public class ScopedTaskQueueService : BackgroundService
    {
        private readonly ILogger _logger;
        public ScopedTaskQueueService(IServiceProvider serviceProvider, IBackgroundTaskQueue backgroundTaskQueue,
            ILoggerFactory loggerFactory)
        {
            ServiceProvider = serviceProvider;
            TaskQueue = backgroundTaskQueue;
            _logger = loggerFactory.CreateLogger<ScopedTaskQueueService>();
        }

        public IServiceProvider ServiceProvider { get; }
        public IBackgroundTaskQueue TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{service} Queued Hosted Service is starting.", nameof(ScopedTaskQueueService));

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueScopedWorkItemAsync(stoppingToken);

                try
                {
                    using (var scope = ServiceProvider.CreateScope())
                    {
                        await workItem(scope, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Scoped Queued Hosted Service is stopping.");
        }
    }
}
