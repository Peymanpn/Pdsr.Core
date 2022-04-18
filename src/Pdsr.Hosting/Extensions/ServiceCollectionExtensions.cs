using Pdsr.Core.Domain;
using Pdsr.Core.User;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Pdsr.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add TaskQueue and Queue Runner services
    /// </summary>
    /// <param name="services">ServiceCollection for services to add to</param>
    public static IServiceCollection RegisterTaskQueue(this IServiceCollection services)
    {

        if (!services.Any(c => c.ServiceType == typeof(ProviderTaskQueueService)))
            services.AddHostedService<ProviderTaskQueueService>();

        if (!services.Any(c => c.ServiceType == typeof(ScopedTaskQueueService)))
            services.AddHostedService<ScopedTaskQueueService>();

        if (!services.Any(c => c.ServiceType == typeof(TaskQueueHostedService)))
            services.AddHostedService<TaskQueueHostedService>();

        if (!services.Any(c => c.ServiceType == typeof(IBackgroundTaskQueue)))
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

        return services;
    }

    public static IServiceCollection RegisterUserTaskQueue<TKey, TUser>(this IServiceCollection services)
        where TKey : notnull
        where TUser : PdsrUserBase<TKey>
    {
        if (!services.Any(c => c.ServiceType == typeof(UserTaskQueuedService<TKey, TUser>)))
            services.AddHostedService<UserTaskQueuedService<TKey, TUser>>();

        return services;
    }

    /// <summary>
    /// Registers all TaskQueue services included in <see cref="RegisterTaskQueue(IServiceCollection)"/>
    /// plus generic background task queue runner <see cref="GenericUserTaskQueuedService{TKey, TUser, TSession, TUserService}"/>
    /// <seealso cref="IUserServiceBase{TKey, TUser}"/>
    /// <seealso cref="ISubjectOwnerProvider"/>
    /// <seealso cref="PdsrUserBase{TKey}"/>
    /// </summary>
    /// <typeparam name="TKey">Type of the Key being used as pk in PdsrUserBase</typeparam>
    /// <typeparam name="TUser">Type of user</typeparam>
    /// <typeparam name="TSession">Type Of session</typeparam>
    /// <typeparam name="TUserService">Type of User Service</typeparam>
    /// <param name="services">ServiceCollection to add services to</param>
    public static IServiceCollection RegisterGenericTaskQueue<TKey, TUser, TSession, TUserService>(this IServiceCollection services)
                where TKey : notnull
                where TUserService : IUserServiceBase<TKey, TUser>
                where TSession : ISubjectOwnerProvider
                where TUser : PdsrUserBase<TKey>
    {
        services.AddHostedService<GenericUserTaskQueuedService<TKey, TUser, TSession, TUserService>>();
        services.RegisterTaskQueue();

        return services;
    }
}
