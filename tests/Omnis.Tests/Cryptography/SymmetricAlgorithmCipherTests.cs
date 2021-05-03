using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Omnis.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace Omnis.Tests.Cryptography {
    public class SymmetricAlgorithmCipherTests : FeatureFixture {
        private SymmetricAlgorithm _algorithm;
        private CryptoStreamMode _mode;

        private byte[] _decryptedData;
        private byte[] _encryptedData;

        public static IEnumerable<object[]> InputDataLengths = new[] {
            new object[] { 0 },
            new object[] { 1 },
            new object[] { 16 },
            new object[] { 1000 },
            new object[] { 52428800 }
        };

        [Scenario]
        [MemberData(nameof(InputDataLengths))]
        public async Task Encrypt_data_using_read_mode(int dataLength) {
            await Runner.RunScenarioAsync(
                _ => Given_symmetric_algorithm(new AesManaged()),
                _ => Given_crypto_stream_mode(CryptoStreamMode.Read),
                _ => When_encrypting_n_bytes(dataLength),
                _ => Then_data_is_correctly_encrypted()
            );
        }

        [Scenario]
        [MemberData(nameof(InputDataLengths))]
        public async Task Encrypt_data_using_write_mode(int dataLength) {
            await Runner.RunScenarioAsync(
                _ => Given_symmetric_algorithm(new AesManaged()),
                _ => Given_crypto_stream_mode(CryptoStreamMode.Write),
                _ => When_encrypting_n_bytes(dataLength),
                _ => Then_data_is_correctly_encrypted()
            );
        }

        [Scenario]
        [MemberData(nameof(InputDataLengths))]
        public async Task Decrypt_data_using_read_mode(int dataLength) {
            await Runner.RunScenarioAsync(
                _ => Given_symmetric_algorithm(new AesManaged()),
                _ => Given_crypto_stream_mode(CryptoStreamMode.Read),
                _ => When_decrypting_n_bytes(dataLength),
                _ => Then_data_is_correctly_decrypted()
            );
        }

        [Scenario]
        [InlineData(52428800)]
        public async Task Decrypt_data_using_write_mode(int dataLength) {
            await Runner.RunScenarioAsync(
                _ => Given_symmetric_algorithm(new AesManaged()),
                _ => Given_crypto_stream_mode(CryptoStreamMode.Write),
                _ => When_decrypting_n_bytes(dataLength),
                _ => Then_data_is_correctly_decrypted()
            );
        }

        private async Task Given_symmetric_algorithm(SymmetricAlgorithm algorithm) {
            _algorithm = algorithm;
        }

        private async Task Given_crypto_stream_mode(CryptoStreamMode mode) {
            _mode = mode;
        }

        private async Task When_encrypting_n_bytes(int n) {
            _decryptedData = GenerateDecryptedData(n);
        }

        private async Task When_decrypting_n_bytes(int n) {
            _encryptedData = await GeneratedEncryptedDataAsync(_algorithm, n);
        }

        private async Task Then_data_is_correctly_encrypted() {
            var encryptTask = EncryptAsync(_decryptedData);
            var controlTask = _algorithm.EncryptAsync(_decryptedData);

            var encrypted = await encryptTask;
            var control = await controlTask;

            Assert.NotNull(encrypted);
            // NOTE: Cannot use xunit's Assert.Equal here because for some reason it is glacially slow with large data sets.
            Assert.True(SequencesAreEqual(control, encrypted), "Encrypted sequence does not match the control sequence");
        }

        private async Task Then_data_is_correctly_decrypted() {
            var decryptTask = DecryptAsync(_encryptedData);
            var controlTask = _algorithm.DecryptAsync(_encryptedData);

            var decrypted = await decryptTask;
            var control = await controlTask;

            Assert.NotNull(decrypted);
            // NOTE: Cannot use xunit's Assert.Equal here because for some reason it is glacially slow with large data sets.
            Assert.True(SequencesAreEqual(control, decrypted), "Decrypted sequence does not match the control sequence");
        }

        private async Task<byte[]> EncryptAsync(byte[] clearText) {
            using (var cipher = new SymmetricAlgorithmCipher(_algorithm, false))
            using (var ostream = new MemoryStream())
            using (var istream = new MemoryStream(clearText)) {
                if (_mode == CryptoStreamMode.Read) {
                    using (var cstream = cipher.CreateEncryptionStream(istream, _mode))
                        await cstream.CopyToAsync(ostream);
                }
                else {
                    using (var cstream = cipher.CreateEncryptionStream(ostream, _mode))
                        await istream.CopyToAsync(cstream);
                }

                return ostream.ToArray();
            }
        }

        private async Task<byte[]> DecryptAsync(byte[] encrypted) {
            using (var cipher = new SymmetricAlgorithmCipher(_algorithm, false))
            using (var ostream = new MemoryStream())
            using (var istream = new MemoryStream(encrypted)) {
                if (_mode == CryptoStreamMode.Read) {
                    using (var cstream = cipher.CreateDecryptionStream(istream, _mode))
                        await cstream.CopyToAsync(ostream);
                }
                else {
                    using (var cstream = cipher.CreateDecryptionStream(ostream, _mode))
                        await istream.CopyToAsync(cstream);
                }

                return ostream.ToArray();
            }
        }

        private static bool SequencesAreEqual(byte[] expected, byte[] actual) {
            if (expected.Length != actual.Length)
                return false;

            for (var i = 0; i < expected.Length; ++i) {
                if (expected[i] != actual[i])
                    return false;
            }

            return true;
        }

        private static byte[] GenerateDecryptedData(int len) {
            var buffer = new byte[len];
            SecureStaticRandom.GetBytes(buffer);
            return buffer;
        }

        private static Task<byte[]> GeneratedEncryptedDataAsync(SymmetricAlgorithm algorithm, int len) {
            var decrypted = GenerateDecryptedData(len);
            return algorithm.EncryptAsync(decrypted);
        }
    }
}
