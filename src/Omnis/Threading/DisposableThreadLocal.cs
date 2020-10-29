using System;
using System.Threading;

namespace Omnis.Threading {
    public class DisposableThreadLocal<T> : ThreadLocal<T> {
        private readonly Action<T> _disposer;
        private bool _disposed;

        public DisposableThreadLocal(Func<T> valueFactory, Action<T> disposer) : base(valueFactory) {
            _disposer = disposer;
            _disposed = false;
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed && _disposer != null) {
                _disposed = true;
                _disposer(Value);
            }

            base.Dispose(disposing);
        }
    }
}
