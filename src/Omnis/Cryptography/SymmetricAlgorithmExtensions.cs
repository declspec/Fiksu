using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Omnis.Cryptography {
    public static class SymmetricAlgorithmExtensions {
        /// <summary>
        /// Creates a CryptoStream that can be used to read/write decrypted data to/from.
        /// </summary>
        /// <param name="algorithm">The algorithm to use for decryption</param>
        /// <param name="target">The stream on which to perform the decryption</param>
        /// <param name="mode">One of the enumeration values that specifies whether to read or write the cryptographic stream</param>
        /// <param name="leaveOpen">True to leave the underlying stream open, false otherwise. Defaults to true</param>
        /// <returns>A CryptoStream to be used for decryption</returns>
        public static CryptoStream CreateDecryptionStream(this SymmetricAlgorithm algorithm, Stream target, CryptoStreamMode mode, bool leaveOpen = true) {
            if (algorithm == null || target == null)
                throw new ArgumentNullException(algorithm == null ? nameof(algorithm) : nameof(target));

            return new ExtendedCryptoStream(target, algorithm.CreateDecryptor(), mode, leaveOpen, true);
        }

        /// <summary>
        /// Creates a CryptoStream that can be used to read/write encrypted data to/from.
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="target">The stream on which to perform the encryption</param>
        /// <param name="mode">One of the enumeration values that specifies whether to read or write the cryptographic stream</param>
        /// <param name="leaveOpen">True to leave the underlying stream open, false otherwise. Defaults to true</param>
        /// <returns>A <see cref="CryptoStream"/> to be used for encryption</returns>
        public static CryptoStream CreateEncryptionStream(this SymmetricAlgorithm algorithm, Stream target, CryptoStreamMode mode, bool leaveOpen = true) {
            if (algorithm == null || target == null)
                throw new ArgumentNullException(algorithm == null ? nameof(algorithm) : nameof(target));

            return new ExtendedCryptoStream(target, algorithm.CreateEncryptor(), mode, leaveOpen, true);
        }

        /// <summary>
        /// Encrypt an array of data
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="data">An array of data to encrypt</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encrypt(this SymmetricAlgorithm algorithm, byte[] data) {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (var outstream = new MemoryStream()) {
                using (var cryptoStream = CreateEncryptionStream(algorithm, outstream, CryptoStreamMode.Write))
                    cryptoStream.Write(data, 0, data.Length);

                return outstream.ToArray();
            }
        }

        /// <summary>
        /// Asynchronously encrypt an array of data
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="data">An array of data to encrypt</param>
        /// <returns>The encrypted data</returns>
        public static async Task<byte[]> EncryptAsync(this SymmetricAlgorithm algorithm, byte[] data) {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (var outstream = new MemoryStream()) {
                using (var cryptoStream = CreateEncryptionStream(algorithm, outstream, CryptoStreamMode.Write))
                    await cryptoStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);

                return outstream.ToArray();
            }
        }

        /// <summary>
        /// Encrypt a stream of data into an output stream
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="input">The input stream containing the original data</param>
        /// <param name="output">The output stream to write the encrypted data to</param>
        public static void Encrypt(this SymmetricAlgorithm algorithm, Stream input, Stream output) {
            using (var cryptostream = CreateEncryptionStream(algorithm, input, CryptoStreamMode.Read))
                cryptostream.CopyTo(output);
        }

        /// <summary>
        /// Encrypt a stream of data into an output stream
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="input">The input stream containing the original data</param>
        /// <param name="output">The output stream to write the encrypted data to</param>
        /// <returns>A <see cref="Task">Task</see> that will resolve once the encryption has been completed</returns>
        public static async Task EncryptAsync(this SymmetricAlgorithm algorithm, Stream input, Stream output) {
            using (var cryptostream = CreateEncryptionStream(algorithm, input, CryptoStreamMode.Read))
                await cryptostream.CopyToAsync(output).ConfigureAwait(false);
        }

        /// <summary>
        /// Decrypt an array of previously encrypted data
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="encrypted">An array of encrypted bytes to decrypt</param>
        /// <returns>The decrypted data</returns>
        public static byte[] Decrypt(this SymmetricAlgorithm algorithm, byte[] encrypted) {
            if (encrypted == null)
                throw new ArgumentNullException(nameof(encrypted));

            using (var outstream = new MemoryStream()) {
                using (var cryptoStream = CreateDecryptionStream(algorithm, outstream, CryptoStreamMode.Write))
                    cryptoStream.Write(encrypted, 0, encrypted.Length);

                return outstream.ToArray();
            }
        }

        /// <summary>
        /// Asynchronously decrypt an array of previously encrypted data
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="encrypted">An array of encrypted bytes to decrypt</param>
        /// <returns>The decrypted data</returns>
        public static async Task<byte[]> DecryptAsync(this SymmetricAlgorithm algorithm, byte[] encrypted) {
            if (encrypted == null)
                throw new ArgumentNullException(nameof(encrypted));

            using (var outstream = new MemoryStream()) {
                using (var cryptoStream = CreateDecryptionStream(algorithm, outstream, CryptoStreamMode.Write))
                    await cryptoStream.WriteAsync(encrypted, 0, encrypted.Length).ConfigureAwait(false);

                return outstream.ToArray();
            }
        }

        /// <summary>
        /// Decrypt a stream on previously encrypted data to an output stream
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="input">The input stream containing the encrypted data</param>
        /// <param name="output">The output stream to write the decrypted data to</param>
        public static void Decrypt(this SymmetricAlgorithm algorithm, Stream input, Stream output) {
            using (var cryptostream = CreateDecryptionStream(algorithm, input, CryptoStreamMode.Read))
                cryptostream.CopyTo(output);
        }

        /// <summary>
        /// Asynchronously ecrypt a stream on previously encrypted data to an output stream
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encryption</param>
        /// <param name="input">The input stream containing the encrypted data</param>
        /// <param name="output">The output stream to write the decrypted data to</param>
        /// <returns>A <see cref="Task">Task</see> that will resolve once the decryption has been completed</returns>
        public static async Task DecryptAsync(this SymmetricAlgorithm algorithm, Stream input, Stream output) {
            using (var cryptostream = CreateDecryptionStream(algorithm, input, CryptoStreamMode.Read))
                await cryptostream.CopyToAsync(output).ConfigureAwait(false);
        }
    }
}