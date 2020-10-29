using Omnis.Cryptography;
using System;
using System.Threading;

namespace Omnis {
    public static class StaticRandom {
        private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => {
            var buffer = new byte[sizeof(int)];
            SecureStaticRandom.GetBytes(buffer);
            return new Random(BitConverter.ToInt32(buffer, 0));
        });

        public static int Next() => _random.Value.Next();
        public static int Next(int minValue) => _random.Value.Next(minValue);
        public static int Next(int minValue, int maxValue) => _random.Value.Next(minValue, maxValue);
        public static double NextDouble() => _random.Value.NextDouble();
        public static void NextBytes(byte[] buffer) => _random.Value.NextBytes(buffer);
    }
}