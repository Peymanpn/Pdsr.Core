
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
