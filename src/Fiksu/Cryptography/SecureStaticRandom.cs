using System.Security.Cryptography;
using System.Threading;

namespace Fiksu.Cryptography {
    public static class SecureStaticRandom {
        private static RNGCryptoServiceProvider _provider = null;

        private static RNGCryptoServiceProvider Provider {
            get {
                if (_provider == null) {
                    var generator = new RNGCryptoServiceProvider();
                    var current = Interlocked.CompareExchange(ref _provider, generator, null);

                    // Check if we lost the race and if so, dispose the provider we created.
                    if (current != null)
                        generator.Dispose();
                }

                return _provider;
            }
        }

        public static void GetBytes(byte[] data) {
            Provider.GetBytes(data);
        }

        public static void GetBytes(byte[] data, int offset, int count) {
            Provider.GetBytes(data, offset, count);
        }
    }
}
