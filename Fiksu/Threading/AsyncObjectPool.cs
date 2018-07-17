using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fiksu.Threading {
    public class AsyncObjectPool<T> where T : class {
        private const int Borrowed = 1;
        private const int Available = 0;

        private static readonly Func<Task<T>, object, T> Disposer = (task, state) => {
            ((IDisposable)state).Dispose();
            return task.Result;
        };

        private static readonly Action<object> Canceller = (state) => {
            ((TaskCompletionSource<T>)state).TrySetCanceled();
        };

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Queue<TaskCompletionSource<T>> _waiting = new Queue<TaskCompletionSource<T>>();
        private readonly PoolRef[] _pool;
        private readonly Func<int, T> _factory;
        private readonly Action<int, T> _finalizer;

        public AsyncObjectPool(int capacity, Func<int, T> factory)
            : this(capacity, factory, null) { }

        public AsyncObjectPool(int capacity, Func<int, T> factory, Action<int, T> finalizer) {
            _pool = new PoolRef[capacity];
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _finalizer = finalizer;
        }

        public Task<T> AcquireAsync(CancellationToken token) {
            // Fast path; there's an available object
            for (var i = 0; i < _pool.Length; ++i) {
                if (Interlocked.CompareExchange(ref _pool[i].State, Borrowed, Available) == Available)
                    return _pool[i].Task ?? (_pool[i].Task = Task.FromResult(_factory(i)));
            }

            var wait = _semaphore.WaitAsync(token);
            return wait.IsCompleted ? EnqueueWaiter(token) : EnqueueWaiterAfter(wait, token);
        }

        public void Release(T obj) {
            // TODO: Make Release return a Task? Or just leave this synchronous
            var pos = FindObjectIndex(obj);
            _semaphore.Wait();

            try {
                // Try and give it away to the first incomplete waiter
                TaskCompletionSource<T> tcs = null;

                while (_waiting.Count > 0) {
                    tcs = _waiting.Dequeue();

                    if (tcs.TrySetResult(obj))
                        break;
                }

                // If nothing was waiting, indicate that the slot is now free.
                if (tcs == null)
                    _pool[pos].State = Available;
            }
            finally {
                _semaphore.Release();
            }
        }

        public void Remove(T obj) {
            // TODO: Make Remove return a Task? Or just leave this synchronous
            var pos = FindObjectIndex(obj);
            _finalizer?.Invoke(pos, obj);
            _semaphore.Wait();

            try {
                // Should we give away the slot?
                TaskCompletionSource<T> tcs = null;
                T newObj = null;

                while (_waiting.Count > 0) {
                    tcs = _waiting.Dequeue();

                    // Ensure the factory is only called once
                    if (newObj == null) {
                        newObj = _factory(pos);
                        _pool[pos].Task = Task.FromResult(newObj);
                    }

                    if (tcs.TrySetResult(newObj))
                        break;
                }

                // If nothing was waiting, indicate that the slot is now free.
                if (tcs == null)
                    _pool[pos].State = Available;
            }
            finally {
                _semaphore.Release();
            }
        }

        // This should only ever be called after a successful call to _semaphore.Wait/WaitAsync
        private Task<T> EnqueueWaiter(CancellationToken token) {
            var tcs = new TaskCompletionSource<T>();
            _waiting.Enqueue(tcs);
            _semaphore.Release();

            if (!token.CanBeCanceled)
                return tcs.Task;

            // Need to properly manage the disposable 'Register'
            var disposable = token.Register(Canceller, tcs);
            return tcs.Task.ContinueWith(Disposer, disposable, TaskContinuationOptions.ExecuteSynchronously);
        }

        private async Task<T> EnqueueWaiterAfter(Task first, CancellationToken token) {
            await first.ConfigureAwait(false);
            return await EnqueueWaiter(token).ConfigureAwait(false);
        }

        private int FindObjectIndex(T obj) {
            for (var pos = 0; pos < _pool.Length; ++pos) {
                if (_pool[pos].Task.Result == obj)
                    return pos;
            }

            throw new InvalidOperationException("Object is not part of the pool, or is not currently borrowed");
        }

        private struct PoolRef {
            public Task<T> Task;
            public int State;
        }
    }
}