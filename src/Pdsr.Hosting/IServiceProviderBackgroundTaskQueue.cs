
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

    /// <summary>
    /// Dequeues a task from queue
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Func<IServiceProvider, CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken);
}
