using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pdsr.Core.Domain;
using Pdsr.Core.User;

namespace Pdsr.Hosting;


/// <summary>
/// Generic user task queue executer.
/// <see cref="PdsrUserBase{TKey}"/>, <see cref="IUserServiceBase{TKey, TUser}"/> and <seealso cref="ISubjectOwnerProvider"/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUser"></typeparam>
/// <typeparam name="TSession"></typeparam>
/// <typeparam name="TUserService"></typeparam>
public class GenericUserTaskQueuedService<TKey, TUser, TSession, TUserService> : BackgroundService
    where TKey : notnull
    where TUserService : IUserServiceBase<TKey, TUser>
    where TSession : ISubjectOwnerProvider
    where TUser : PdsrUserBase<TKey>

{
    private readonly ILogger _logger;
    public GenericUserTaskQueuedService(IServiceProvider serviceProvider,
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
        _logger.LogInformation("{service} Queued Hosted Service is starting.", nameof(GenericUserTaskQueuedService<TKey, TUser, TSession, TUserService>));

        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await TaskQueue.DequeueUserTaskAsync(stoppingToken);

            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<TUserService>();
                    var session = scope.ServiceProvider.GetRequiredService<TSession>();

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
