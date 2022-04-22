using Pdsr.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Hosting;

/// <summary>
/// TaskQueue
/// </summary>
public interface IBackgroundTaskQueue : IBackgroundTaskQueue<string, PdsrUserBase<string>>
{

}

/// <summary>
/// TaskQueue
/// </summary>
public interface IBackgroundTaskQueue<TKey, TUser>
    where TKey : notnull
    where TUser : PdsrUserBase<TKey>
{
    /// <summary>
    /// Simplest form of task to run in queue
    /// </summary>
    /// <param name="workItem">a simple task to run in queue</param>
    void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Queue a task with auto IServiceProver inject
    /// </summary>
    /// <param name="workItem"></param>
    void QueueBackgroundProviderWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem);
    Task<Func<IServiceProvider, CancellationToken, Task>> DequeueProviderTaskAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Queue a task with auto IServiceScope Inject
    /// </summary>
    /// <param name="workItem"></param>
    void QueueBackgroundScopedWorkItem(Func<IServiceScope, CancellationToken, Task> workItem);
    Task<Func<IServiceScope, CancellationToken, Task>> DequeueScopedWorkItemAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Queues a User Specifiec Task for systems already implemented <see cref="Pdsr.Core.User.IUserServiceBase{TKey, TUser}"/>,
    /// <see cref="PdsrUserBase"/> and <see cref="ISubjectOwnable"/> which is a superset of ISessionBase
    /// </summary>
    /// <param name="sub"></param>
    /// <param name="workItem"></param>
    void QueueUserWorkItem(string sub, Func<IServiceProvider, TUser, CancellationToken, Task> workItem);

    Task<KeyValuePair<string, Func<IServiceProvider, TUser, CancellationToken, Task>>> DequeueUserTaskAsync(CancellationToken cancellationToken);
}



public interface ISimpleBackgroundTaskQueue
{
    /// <summary>
    /// Simplest form of task to run queued
    /// </summary>
    /// <param name="workItem">a simple task to run in queue</param>
    void QueueWorkItem(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken);
}

/// <summary>
/// Run work Items with already created ServiceScope.
/// </summary>
public interface IServiceProviderBackgroundTaskQueue
{
    /// <summary>
    /// Queue a task with auto IServiceProver inject
    /// </summary>
    /// <param name="workItem"></param>
    void QueueWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem);
    Task<Func<IServiceProvider, CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken);
}


/// <summary>
/// TaskQueue to add Tasks
/// </summary>
public interface IUserBackgroundTaskQueue<TKey, TUser>
    where TKey : notnull
    where TUser : PdsrUserBase<TKey>, ISubjectOwnable
{
    /// <summary>
    /// Queues a User Specifiec Task for systems already implemented <see cref="Pdsr.Core.User.IUserServiceBase{TKey, TUser}"/>,
    /// <see cref="PdsrUserBase"/> and <see cref="ISubjectOwnable"/> which is a superset of ISessionBase and Session
    /// </summary>
    /// <param name="sub"></param>
    /// <param name="workItem"></param>
    void QueueUserWorkItem(string sub, Func<IServiceProvider, TUser, CancellationToken, Task> workItem);
    Task<KeyValuePair<string, Func<IServiceProvider, TUser, CancellationToken, Task>>> DequeueUserTaskAsync(CancellationToken cancellationToken);
}
