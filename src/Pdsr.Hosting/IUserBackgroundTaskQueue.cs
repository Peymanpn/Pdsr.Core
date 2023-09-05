using Pdsr.Core.Domain;


/// <summary>
/// User TaskQueue.
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
