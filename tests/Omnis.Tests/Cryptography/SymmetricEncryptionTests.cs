using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Omnis.Cryptography;
using Xunit;

namespace Omnis.Tests.Cryptography {
    public class SymmetricEncryptionTests {
        private static readonly Random Rng = new Random();

        [Fact]
        public async void ShouldThrowOnNullInputs() {
            Assert.Throws<ArgumentNullException>(() => CreateAlgorithm().Encrypt(null));
            Assert.Throws<ArgumentNullException>(() => (null as SymmetricAlgorithm).Encrypt(new byte[10]));
            await Assert.ThrowsAsync<ArgumentNullException>(() => CreateAlgorithm().EncryptAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => (null as SymmetricAlgorithm).EncryptAsync(new byte[10]));

            Assert.Throws<ArgumentNullException>(() => CreateAlgorithm().Decrypt(null));
            Assert.Throws<ArgumentNullException>(() => (null as SymmetricAlgorithm).Decrypt(new byte[10]));
            await Assert.ThrowsAsync<ArgumentNullException>(() => CreateAlgorithm().DecryptAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => (null as SymmetricAlgorithm).DecryptAsync(new byte[10]));

            Assert.Throws<ArgumentNullException>(() => CreateAlgorithm().CreateEncryptionStream(null, CryptoStreamMode.Read));
            Assert.Throws<ArgumentNullException>(() => (null as SymmetricAlgorithm).CreateEncryptionStream(new MemoryStream(), CryptoStreamMode.Read));
            Assert.Throws<ArgumentNullException>(() => CreateAlgorithm().CreateDecryptionStream(null, CryptoStreamMode.Read));
            Assert.Throws<ArgumentNullException>(() => (null as SymmetricAlgorithm).CreateDecryptionStream(new MemoryStream(), CryptoStreamMode.Read));
        }

        [Theory]
        [MemberData(nameof(DataLengths))]
        public void ShouldCorrectlyEncryptAndDecryptByteArrays(int len) {
            var algorithm = CreateAlgorithm();
            var data = new byte[len];
            Rng.NextBytes(data);

            var encrypted = algorithm.Encrypt(data);
            Assert.False(encrypted.SequenceEqual(data)); // ensure encryption actually happened

            var decrypted = algorithm.Decrypt(encrypted);
            Assert.True(decrypted.SequenceEqual(data));
        }

        [Theory]
        [MemberData(nameof(DataLengths))]
        public void ShouldCorrectlyEncryptAndDecryptStreams(int len) {
            var algorithm = CreateAlgorithm();
            var data = new byte[len];
            Rng.NextBytes(data);

            using (var inputStream = new MemoryStream(data))
            using (var encStream = new MemoryStream()) {
                algorithm.Encrypt(inputStream, encStream);

                // Ensure encryption actually happened.
                var encrypted = encStream.ToArray();
                Assert.False(data.SequenceEqual(encrypted));
                encStream.Position = 0;

                using (var decStream = new MemoryStream()) {
                    algorithm.Decrypt(encStream, decStream);
                    Assert.True(data.SequenceEqual(decStream.ToArray()));
                }
            }
        }

        [Theory]
        [MemberData(nameof(DataLengths))]
        public async Task ShouldCorrectlyEncryptAndDecryptByteArraysAsync(int len) {
            var algorithm = CreateAlgorithm();
            var data = new byte[len];
            Rng.NextBytes(data);

            var encrypted = await algorithm.EncryptAsync(data);
            Assert.False(encrypted.SequenceEqual(data)); // ensure encryption actually happened

            var decrypted = await algorithm.DecryptAsync(encrypted);
            Assert.True(decrypted.SequenceEqual(data));
        }

        [Theory]
        [MemberData(nameof(DataLengths))]
        public async Task ShouldCorrectlyEncryptAndDecryptStreamsAsync(int len) {
            var algorithm = CreateAlgorithm();
            var data = new byte[len];
            Rng.NextBytes(data);

            using (var inputStream = new MemoryStream(data))
            using (var encStream = new MemoryStream()) {
                await algorithm.EncryptAsync(inputStream, encStream);

                // Ensure encryption actually happened.
                var encrypted = encStream.ToArray();
                Assert.False(data.SequenceEqual(encrypted));
                encStream.Position = 0;

                using (var decStream = new MemoryStream()) {
                    await algorithm.DecryptAsync(encStream, decStream);
                    Assert.True(data.SequenceEqual(decStream.ToArray()));
                }
            }
        }

        private static SymmetricAlgorithm CreateAlgorithm() {
            var key = new byte[32];
            var iv = new byte[16];

            Rng.NextBytes(key);
            Rng.NextBytes(iv);

            return new AesManaged() {
                IV = iv,
                Key = key
            };
        }

        public static IEnumerable<object[]> DataLengths = new[]
        {
            new object[] { 0 },
            new object[] { 1 },
            new object[] { 45 },
            new object[] { 1203 },
            new object[] { 8511025 },
            new object[] { 27664665 } // ~25 MB
        };
    }
}
