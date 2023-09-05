
/// <summary>
/// Simplest form of task queue
/// </summary>
public interface ISimpleBackgroundTaskQueue
{
    /// <summary>
    /// Simplest form of task to run queued
    /// </summary>
    /// <param name="workItem">a simple task to run in queue</param>
    void QueueWorkItem(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>> DequeueTask(CancellationToken cancellationToken);
}

