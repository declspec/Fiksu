using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fiksu.Threading {
    public struct PooledObject<T> : IDisposable {
        public T Value { get; }
        public int Index { get; private set; }

        private readonly AsyncObjectPool<T> _pool;

        internal PooledObject(T value, int poolIndex, AsyncObjectPool<T> pool) {
            Value = value;
            Index = poolIndex;
            _pool = pool;
        }

        public void Destroy() {
            Release(true);
        }

        public void Dispose() {
            Release(false);
        }

        private void Release(bool destroy) {
            if (Index >= 0 && _pool != null) {
                var index = Index;
                Index = -1;
                _pool.Release(index, destroy);
            }
        }
    }

    public class AsyncObjectPool<T> {
        private static readonly Task<bool> SuccessfulTask = Task.FromResult(true);

        private const int ObjectUnset = 0;
        private const int ObjectFree = 1;
        private const int ObjectBorrowed = 2;

        private readonly PoolRef[] _refs;
        private readonly Func<int, T> _factory;

        private TaskCompletionSource<bool> _waiter;

        public AsyncObjectPool(int capacity, Func<int, T> generator) {
            _factory = generator ?? throw new ArgumentNullException(nameof(generator));
            // TODO: Could do some work here around creating a sparse array
            //  to avoid allocating all the references up-front, but this is far simpler
            _refs = new PoolRef[capacity];
        }

        public bool TryAquire(out PooledObject<T> value) {
            // Try and be somewhat fair and avoid out-racing anyone who is already waiting
            if (_waiter == null) {
                for (var i = 0; i < _refs.Length; ++i) {
                    var oldState = _refs[i].State;

                    if (oldState < ObjectBorrowed && Interlocked.CompareExchange(ref _refs[i].State, ObjectBorrowed, oldState) == oldState) {
                        if (oldState == ObjectUnset) {
                            try {
                                _refs[i].Object = _factory(i);
                            }
                            catch {
                                // Avoid locking the space in the pool
                                _refs[i].State = ObjectUnset;
                                throw;
                            }
                        }

                        value = new PooledObject<T>(_refs[i].Object, i, this);
                        return true;
                    }
                }

                // If there were no available items in the pool, create a TaskCompletionSource
                //  to be used by WaitToAcquireAsync
                Interlocked.CompareExchange(ref _waiter, new TaskCompletionSource<bool>(), null);
            }

            value = default(PooledObject<T>);
            return false;
        }

        public Task<bool> WaitForReleaseAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<bool>(cancellationToken);

            // Keep a local reference to whichever TaskCompletionSource we grab
            // to avoid racing later if the source changes partway through this method
            var localWaiter = _waiter;

            if (localWaiter == null)
                return SuccessfulTask;

            if (!cancellationToken.CanBeCanceled || localWaiter.Task.IsCompleted)
                return localWaiter.Task;

            // TODO: Look at better / more efficient ways of doing this
            return localWaiter.Task.ContinueWith(t => t.IsCompleted && t.Result, cancellationToken);
        }

        public async Task<PooledObject<T>> AcquireAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            PooledObject<T> value;

            while (!TryAquire(out value)) {
                if (!await WaitForReleaseAsync(cancellationToken).ConfigureAwait(false))
                    throw new ObjectDisposedException(nameof(AsyncObjectPool<T>));
            }

            return value;
        }

        // TODO: Because this method is 'internal' (so it can be used by PoolObject), there is a chance
        //  a bad method from the same library could corrupt the internal state with this method.
        //  Would be nicer to have some safety on it but I can't think of anything simple right now,
        //  especially with the limited scope.
        internal void Release(int index, bool removeFromPool) {
            // Update the state so other callers can use the object
            //  then trigger the pending task (if it exists).
            if (!removeFromPool)
                _refs[index].State = ObjectFree;
            else {
                _refs[index].Object = default;
                _refs[index].State = ObjectUnset;
            }

            Interlocked.Exchange(ref _waiter, null)?.SetResult(true);
        }

        private struct PoolRef {
            public int State;
            public T Object;
        }
    }
}
