using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Hosting
{
    public class ProviderTaskQueueService : BackgroundService
    {
        private readonly ILogger _logger;
        public ProviderTaskQueueService(IServiceProvider serviceProvider, IBackgroundTaskQueue backgroundTaskQueue,
            ILoggerFactory loggerFactory)
        {
            ServiceProvider = serviceProvider;
            TaskQueue = backgroundTaskQueue;
            _logger = loggerFactory.CreateLogger<ProviderTaskQueueService>();
        }

        public IServiceProvider ServiceProvider { get; }
        public IBackgroundTaskQueue TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{service} Queued Hosted Service is starting.", nameof(ProviderTaskQueueService));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace("Attempting to dequeue work item.");
                var workItem = await TaskQueue.DequeueProviderTaskAsync(stoppingToken);
                _logger.LogTrace("WorkItem {workItem} Dequeued successfully.", nameof(workItem));
                try
                {
                    using (var scope = ServiceProvider.CreateScope())
                    {
                        _logger.LogTrace("Executing work item, {workItem} and providing service provider", nameof(workItem));
                        await workItem(scope.ServiceProvider, stoppingToken);
                        _logger.LogTrace("Work item completing, destroying scope.");
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
