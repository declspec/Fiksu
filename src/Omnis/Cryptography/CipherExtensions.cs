using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Omnis.Cryptography {
    public static class CipherExtensions {
        /// <summary>
        /// Read cleartext data from an input stream and write the encrypted data to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for encrypting the data</param>
        /// <param name="input">The input stream containing cleartext data to read from</param>
        /// <param name="output">The output stream to write the encrypted data to</param>
        /// <remarks>This method leaves both the <paramref name="input"/> and <paramref name="output"/> streams open</remarks>
        public static void Encrypt(this ICipher cipher, Stream input, Stream output) {
            using (var cryptoStream = cipher.CreateEncryptionStream(output, CryptoStreamMode.Write, true))
                input.CopyTo(cryptoStream);
        }

        /// <summary>
        /// Read cleartext data from an input stream and write the encrypted data to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for encrypting the data</param>
        /// <param name="input">The input stream containing cleartext data to read from</param>
        /// <param name="output">The output stream to write the encrypted data to</param>
        /// <remarks>This method leaves both the <paramref name="input"/> and <paramref name="output"/> streams open</remarks>
        public static async Task EncryptAsync(this ICipher cipher, Stream input, Stream output) {
            using (var cryptoStream = cipher.CreateEncryptionStream(output, CryptoStreamMode.Write, true))
                await input.CopyToAsync(cryptoStream).ConfigureAwait(false);
        }

        /// <summary>
        /// Read encrypted data from an input stream and write the decrypted data to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for decrypting the data</param>
        /// <param name="input">The input stream containing encrypted data to read from</param>
        /// <param name="output">The output stream to write the decrypted data to</param>
        /// <remarks>This method leaves both the <paramref name="input"/> and <paramref name="output"/> streams open</remarks>
        public static void Decrypt(this ICipher cipher, Stream input, Stream output) {
            using (var cryptoStream = cipher.CreateDecryptionStream(input, CryptoStreamMode.Read, true))
                cryptoStream.CopyTo(output);
        }

        /// <summary>
        /// Read encrypted data from an input stream and write the decrypted data to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for decrypting the data</param>
        /// <param name="input">The input stream containing encrypted data to read from</param>
        /// <param name="output">The output stream to write the decrypted data to</param>
        /// <remarks>This method leaves both the <paramref name="input"/> and <paramref name="output"/> streams open</remarks>
        public static async Task DecryptAsync(this ICipher cipher, Stream input, Stream output) {
            using (var cryptoStream = cipher.CreateDecryptionStream(input, CryptoStreamMode.Read, true))
                await cryptoStream.CopyToAsync(output).ConfigureAwait(false);
        }

        /// <summary>
        /// Encrypt the cleartext contents of a byte array
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for encrypting the data</param>
        /// <param name="input">The cleartext data to encrypt</param>
        /// <returns>An array containing the encrypted data</returns>
        public static byte[] Encrypt(this ICipher cipher, byte[] input) {
            using (var ostream = new MemoryStream()) {
                cipher.Encrypt(input, ostream);
                return ostream.ToArray();
            }
        }

        /// <summary>
        /// Encrypt the cleartext contents of a byte array
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for encrypting the data</param>
        /// <param name="input">The cleartext data to encrypt</param>
        /// <returns>An array containing the encrypted data</returns>
        public static async Task<byte[]> EncryptAsync(this ICipher cipher, byte[] input) {
            using (var istream = new MemoryStream(input))
            using (var ostream = new MemoryStream()) {
                await cipher.EncryptAsync(istream, ostream).ConfigureAwait(false);
                return ostream.ToArray();
            }
        }

        /// <summary>
        /// Encrypt the cleartext contents of a byte array and write the contents to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for encrypting the data</param>
        /// <param name="input">The cleartext data to encrypt</param>
        /// <param name="output">The output stream to write the encrypted data to</param>
        /// <remarks>This method leave the <paramref name="output"/> stream open</remarks>
        public static void Encrypt(this ICipher cipher, byte[] input, Stream output) {
            using (var istream = new MemoryStream(input))
                cipher.Encrypt(istream, output);
        }

        /// <summary>
        /// Encrypt the cleartext contents of a byte array and write the contents to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for encrypting the data</param>
        /// <param name="input">The cleartext data to encrypt</param>
        /// <param name="output">The output stream to write the encrypted data to</param>
        /// <remarks>This method leave the <paramref name="output"/> stream open</remarks>
        public static async Task EncryptAsync(this ICipher cipher, byte[] input, Stream output) {
            using (var istream = new MemoryStream(input))
                await cipher.EncryptAsync(istream, output).ConfigureAwait(false);
        }

        /// <summary>
        /// Decrypt the encrypted contents of a byte array
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for decrypting the data</param>
        /// <param name="input">The encrypted data to decrypt</param>
        /// <returns>An array containing the cleartext data</returns>
        public static byte[] Decrypt(this ICipher cipher, byte[] input) {
            using (var ostream = new MemoryStream()) {
                cipher.Decrypt(input, ostream);
                return ostream.ToArray();
            }
        }

        /// <summary>
        /// Decrypt the encrypted contents of a byte array
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for decrypting the data</param>
        /// <param name="input">The encrypted data to decrypt</param>
        /// <returns>An array containing the cleartext data</returns>
        public static async Task<byte[]> DecryptAsync(this ICipher cipher, byte[] input) {
            using (var istream = new MemoryStream(input))
            using (var ostream = new MemoryStream()) {
                await cipher.DecryptAsync(istream, ostream).ConfigureAwait(false);
                return ostream.ToArray();
            }
        }

        /// <summary>
        /// Decrypt the encrypted contents of a byte array and writes it to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for decrypting the data</param>
        /// <param name="input">The encrypted data to decrypt</param>
        /// <param name="output">The stream to write the cleartext data to</param>
        /// <remarks>This method leave the <paramref name="output"/> stream open</remarks>
        public static void Decrypt(this ICipher cipher, byte[] input, Stream output) {
            using (var istream = new MemoryStream(input))
                cipher.Decrypt(istream, output);
        }

        /// <summary>
        /// Decrypt the encrypted contents of a byte array and writes it to an output stream
        /// </summary>
        /// <param name="cipher">The <see cref="ICipher"/> to use for decrypting the data</param>
        /// <param name="input">The encrypted data to decrypt</param>
        /// <param name="output">The stream to write the cleartext data to</param>
        /// <remarks>This method leave the <paramref name="output"/> stream open</remarks>
        public static async Task DecryptAsync(this ICipher cipher, byte[] input, Stream output) {
            using (var istream = new MemoryStream(input))
                await cipher.DecryptAsync(istream, output).ConfigureAwait(false);
        }
    }
}