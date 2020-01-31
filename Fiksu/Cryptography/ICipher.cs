using System;
using System.IO;
using System.Security.Cryptography;

namespace Fiksu.Cryptography {
    /// <summary>
    /// Defines the basic operations for cryptographic transformation on streams of data
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are not guaranteed to be thread-safe.
    /// </remarks>
    public interface ICipher : IDisposable {
        /// <summary>
        /// Create a decryption stream from a target stream.
        /// </summary>
        /// <param name="target">The target stream to read from or write to</param>
        /// <param name="mode"><see cref="CryptoStreamMode.Read"/> if <paramref name="target"/> is the source of the encrypted data, <see cref="CryptoStreamMode.Write"/> if <paramref name="target"/> is the destination of the decrypted data.</param>
        /// <param name="leaveOpen">Specify if <paramref name="target"/> should be left open after the operation is completed</param>
        /// <returns>A <see cref="Stream"/> object, depending on the value specified for <paramref name="mode"/> this stream can either be written to or read from</returns>
        Stream CreateDecryptionStream(Stream target, CryptoStreamMode mode, bool leaveOpen = true);

        /// <summary>
        /// Create an encryption stream from a target stream.
        /// </summary>
        /// <param name="target">The target stream to read from or write to</param>
        /// <param name="mode"><see cref="CryptoStreamMode.Read"/> if <paramref name="target"/> is the source of the decrypted data, <see cref="CryptoStreamMode.Write"/> if <paramref name="target"/> is the destination of the encrypted data.</param>
        /// <param name="leaveOpen">Specify if <paramref name="target"/> should be left open after the operation is completed</param>
        /// <returns>A <see cref="Stream"/> object, depending on the value specified for <paramref name="mode"/> this stream can either be written to or read from</returns>
        Stream CreateEncryptionStream(Stream target, CryptoStreamMode mode, bool leaveOpen = true);
    }
}