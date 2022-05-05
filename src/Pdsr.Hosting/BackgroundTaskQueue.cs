using Pdsr.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pdsr.Hosting;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly ConcurrentQueue<KeyValuePair<string, Func<IServiceProvider, PdsrUserBase<string>, CancellationToken, Task>>> _userWorkItems =
        new ConcurrentQueue<KeyValuePair<string, Func<IServiceProvider, PdsrUserBase<string>, CancellationToken, Task>>>();

    private readonly ConcurrentQueue<Func<IServiceProvider, CancellationToken, Task>> _providerWorkItems =
        new ConcurrentQueue<Func<IServiceProvider, CancellationToken, Task>>();

    private readonly ConcurrentQueue<Func<IServiceScope, CancellationToken, Task>> _scopedWorkItems =
        new ConcurrentQueue<Func<IServiceScope, CancellationToken, Task>>();

    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems =
        new ConcurrentQueue<Func<CancellationToken, Task>>();

    private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
    private readonly SemaphoreSlim _signalProvider = new SemaphoreSlim(0);
    private readonly SemaphoreSlim _signalScoped = new SemaphoreSlim(0);
    private readonly SemaphoreSlim _signalUser = new SemaphoreSlim(0);

    public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        _workItems.Enqueue(workItem);
        _signal.Release();
    }

    /// <inheritdoc/>
    public async Task<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);

        _workItems.TryDequeue(out var workItem);

        if (workItem is null)
        {
            throw new NullReferenceException(nameof(workItem));
        }

        return workItem;
    }

    /// <inheritdoc/>
    public void QueueBackgroundProviderWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        _providerWorkItems.Enqueue(workItem);
        _signalProvider.Release();
    }

    /// <inheritdoc/>
    public async Task<Func<IServiceProvider, CancellationToken, Task>> DequeueProviderTaskAsync(CancellationToken cancellationToken)
    {
        await _signalProvider.WaitAsync(cancellationToken);
        _providerWorkItems.TryDequeue(out var workItem);

        if (workItem is null)
        {
            throw new NullReferenceException(nameof(workItem));
        }


        return workItem;
    }

    public void QueueBackgroundScopedWorkItem(Func<IServiceScope, CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        _scopedWorkItems.Enqueue(workItem);
        _signalScoped.Release();
    }

    public async Task<Func<IServiceScope, CancellationToken, Task>> DequeueScopedWorkItemAsync(CancellationToken cancellationToken = default)
    {
        await _signalScoped.WaitAsync(cancellationToken);
        _scopedWorkItems.TryDequeue(out var workItem);

        if (workItem is null)
        {
            throw new NullReferenceException(nameof(workItem));
        }

        return workItem;
    }

    /// <inheritdoc/>
    public void QueueUserWorkItem(string sub, Func<IServiceProvider, PdsrUserBase<string>, CancellationToken, Task> workItem)
    {
        if (string.IsNullOrEmpty(sub) || workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        _userWorkItems.Enqueue(
            new KeyValuePair<string, Func<IServiceProvider, PdsrUserBase<string>, CancellationToken, Task>>(sub, workItem)
            );

        _signalUser.Release();
    }

    public async Task<KeyValuePair<string, Func<IServiceProvider, PdsrUserBase<string>, CancellationToken, Task>>> DequeueUserTaskAsync(CancellationToken cancellationToken)
    {
        await _signalUser.WaitAsync(cancellationToken);

        _userWorkItems.TryDequeue(out var workItem);
        return workItem;
    }
}



/// <summary>
/// User specific Tasl Queue
/// </summary>
/// <typeparam name="TKey">User's Key type. it is usally same as subject id.<see cref="PdsrUserBase{TKey}.SubjectId"/></typeparam>
/// <typeparam name="TUser">The user type being used to load during queue work execution.<see cref="PdsrUserBase{TKey}"/></typeparam>
public class BackgroundTaskQueue<TKey, TUser> : IBackgroundTaskQueue<TKey, TUser>
    where TKey : notnull
    where TUser : PdsrUserBase<TKey>
{
    private readonly ConcurrentQueue<KeyValuePair<string, Func<IServiceProvider, TUser, CancellationToken, Task>>> _userWorkItems =
        new ConcurrentQueue<KeyValuePair<string, Func<IServiceProvider, TUser, CancellationToken, Task>>>();

    private readonly ConcurrentQueue<Func<IServiceProvider, CancellationToken, Task>> _providerWorkItems =
        new ConcurrentQueue<Func<IServiceProvider, CancellationToken, Task>>();

    private readonly ConcurrentQueue<Func<IServiceScope, CancellationToken, Task>> _scopedWorkItems =
        new ConcurrentQueue<Func<IServiceScope, CancellationToken, Task>>();

    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems =
        new ConcurrentQueue<Func<CancellationToken, Task>>();

    private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
    private readonly SemaphoreSlim _signalProvider = new SemaphoreSlim(0);
    private readonly SemaphoreSlim _signalScoped = new SemaphoreSlim(0);
    private readonly SemaphoreSlim _signalUser = new SemaphoreSlim(0);

    /// <inheritdoc/>
    public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        _workItems.Enqueue(workItem);
        _signal.Release();
    }

    /// <inheritdoc/>
    public async Task<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);

        _workItems.TryDequeue(out var workItem);

        if (workItem is null)
        {
            throw new NullReferenceException(nameof(workItem));
        }


        return workItem;
    }
    /// <inheritdoc/>
    public void QueueBackgroundProviderWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        _providerWorkItems.Enqueue(workItem);
        _signalProvider.Release();
    }

    /// <inheritdoc/>
    public async Task<Func<IServiceProvider, CancellationToken, Task>> DequeueProviderTaskAsync(CancellationToken cancellationToken)
    {
        await _signalProvider.WaitAsync(cancellationToken);
        _providerWorkItems.TryDequeue(out var workItem);
        if (workItem is null)
        {
            throw new NullReferenceException(nameof(workItem));
        }

        return workItem;
    }

    /// <inheritdoc/>
    public void QueueBackgroundScopedWorkItem(Func<IServiceScope, CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        _scopedWorkItems.Enqueue(workItem);
        _signalScoped.Release();
    }

    /// <inheritdoc/>
    public async Task<Func<IServiceScope, CancellationToken, Task>> DequeueScopedWorkItemAsync(CancellationToken cancellationToken = default)
    {
        await _signalScoped.WaitAsync(cancellationToken);
        _scopedWorkItems.TryDequeue(out var workItem);
        if (workItem is null)
        {
            throw new NullReferenceException(nameof(workItem));
        }

        return workItem;
    }

    /// <inheritdoc/>
    public void QueueUserWorkItem(string sub, Func<IServiceProvider, TUser, CancellationToken, Task> workItem)
    {
        if (string.IsNullOrEmpty(sub) || workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }
        _userWorkItems.Enqueue(
            new KeyValuePair<string, Func<IServiceProvider, TUser, CancellationToken, Task>>(sub, workItem)
            );

        _signalUser.Release();
    }

    /// <inheritdoc/>
    public async Task<KeyValuePair<string, Func<IServiceProvider, TUser, CancellationToken, Task>>> DequeueUserTaskAsync(CancellationToken cancellationToken)
    {
        await _signalUser.WaitAsync(cancellationToken);

        _userWorkItems.TryDequeue(out var workItem);
        return workItem;
    }
}
