using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fiksu.Threading {

    public class AsyncValuePool<T> {
        private static readonly Task<bool> SuccessfulTask = Task.FromResult(true);

        private static readonly Func<T, T, bool> AreValuesEqual = typeof(T).IsValueType
            ? EqualityComparer<T>.Default.Equals
            : new Func<T, T, bool>((t1, t2) => ReferenceEquals(t1, t2));

        private const int ObjectUnset = 0;
        private const int ObjectFree = 1;
        private const int ObjectBorrowed = 2;

        private readonly PoolRef[] _refs;
        private readonly Func<int, T> _factory;

        private TaskCompletionSource<bool> _waiter;

        public AsyncValuePool(int capacity, Func<int, T> generator) {
            _factory = generator ?? throw new ArgumentNullException(nameof(generator));
            // TODO: Could do some work here around creating a sparse array
            //  to avoid allocating all the references up-front, but this is far simpler
            _refs = new PoolRef[capacity];
        }

        public bool TryAquire(out T value) {
            // Try and be somewhat fair and avoid out-racing anyone who is already waiting
            if (_waiter == null) {
                for (var i = 0; i < _refs.Length; ++i) {
                    var oldState = _refs[i].State;

                    if (oldState < ObjectBorrowed && Interlocked.CompareExchange(ref _refs[i].State, ObjectBorrowed, oldState) == oldState) {
                        if (oldState == ObjectUnset) {
                            try {
                                var nextValue = _factory(i);

                                // TODO: Safety / performance evaluation
                                //  performance would only be affected in two situations:
                                // 1) The pool is still filling up; each call to TryAcquire needs to lock as the objects aren't initialised.
                                // 2) Users of the pool are consistently calling Release with removeFromPool=true; this probably indicates poor usage of a pool
                                //      anyway so isn't likely the biggest concern for performance.
                                // Could be a flag in the constructor: "f(i) -> v is guaranteed to produce a unique set of v values for each unique value of i".
                                lock (_refs) {
                                    // TODO: Could be a space / time consideration for using a HashSet or something to ensure value is unique
                                    //  which would be faster but more space would be required (and the performance would only matter for large pool sizes)
                                    for (var ii = 0; ii < _refs.Length; ++ii) {
                                        if (_refs[ii].State > ObjectUnset && AreValuesEqual(_refs[ii].Object, nextValue))
                                            throw new NotSupportedException("duplicate values are not supported in the pool");
                                    }

                                    _refs[i].Object = nextValue;
                                }
                            }
                            catch {
                                // Avoid locking the space in the pool
                                _refs[i].State = ObjectUnset;
                                throw;
                            }
                        }

                        value = _refs[i].Object;
                        return true;
                    }
                }

                // If there were no available items in the pool, create a TaskCompletionSource
                //  to be used by WaitForReleaseAsync
                Interlocked.CompareExchange(ref _waiter, new TaskCompletionSource<bool>(), null);
            }

            value = default(T);
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

        public async Task<T> AcquireAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            T value;

            while (!TryAquire(out value)) {
                if (!await WaitForReleaseAsync(cancellationToken).ConfigureAwait(false))
                    throw new ObjectDisposedException(nameof(AsyncValuePool<T>));
            }

            return value;
        }

        public void Release(T value, bool removeFromPool = false) {
            for (var i = 0; i < _refs.Length; ++i) {
                if (AreValuesEqual(_refs[i].Object, value)) {
                    // Update the state so other callers can use the object
                    //  then trigger the pending task (if it exists).            
                    Interlocked.Exchange(ref _refs[i].State, removeFromPool ? ObjectUnset : ObjectFree);
                    Interlocked.Exchange(ref _waiter, null)?.SetResult(true);
                    break;
                }
            }

            throw new ArgumentException("value does not belong to the current pool", nameof(value));
        }

        private struct PoolRef {
            public int State;
            public T Object;
        }
    }
}
