namespace Pdsr.Core
{
    /// <summary>
    /// Retrying policy
    /// </summary>
    public interface IRetryPolicy
    {
        void Execute(Action operation);

        TResult Execute<TResult>(Func<TResult> operation);

        Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken);

        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken);
    }
}
