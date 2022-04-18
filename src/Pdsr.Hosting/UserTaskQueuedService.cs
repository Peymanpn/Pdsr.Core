using Pdsr.Core.Domain;
using Pdsr.Core.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Hosting
{
    public class UserTaskQueuedService<TKey, TUser> : BackgroundService
        where TKey : notnull
        where TUser : PdsrUserBase<TKey>
    {
        private readonly ILogger _logger;
        public UserTaskQueuedService(IServiceProvider serviceProvider,
            IBackgroundTaskQueue<TKey, TUser> backgroundTaskQueue,
            ILoggerFactory loggerFactory)
        {
            ServiceProvider = serviceProvider;
            TaskQueue = backgroundTaskQueue;
            _logger = loggerFactory.CreateLogger<ProviderTaskQueueService>();
        }

        public IServiceProvider ServiceProvider { get; }
        public IBackgroundTaskQueue<TKey, TUser> TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scoped Queued Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueUserTaskAsync(stoppingToken);

                try
                {
                    using (var scope = ServiceProvider.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserServiceBase<TKey, TUser>>();
                        var session = scope.ServiceProvider.GetRequiredService<ISubjectOwnerProvider>();
                        session.SetSubjectId(workItem.Key);
                        var user = await userService.GetUserAsync(stoppingToken);

                        if (user is null)
                        {
                            throw new NullReferenceException(nameof(user));
                        }

                        await workItem.Value(scope.ServiceProvider, user, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Scoped Queued Hosted Service is stopping.");
        }
    }
}
