using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Hosting
{
    public class TaskQueueHostedService : BackgroundService
    {
        private readonly ILogger _logger;

        public TaskQueueHostedService(IBackgroundTaskQueue taskQueue,
            IServiceProvider serviceProvider,
            ILoggerFactory logger)
        {
            TaskQueue = taskQueue;
            ServiceProvider = serviceProvider;
            _logger = logger.CreateLogger<TaskQueueHostedService>();
        }

        public IBackgroundTaskQueue TaskQueue { get; }
        public IServiceProvider ServiceProvider { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"Queued Hosted Service is running.{Environment.NewLine}" +
                $"background queue.{Environment.NewLine}");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
