using System.IO;
using System.Security.Cryptography;

namespace Fiksu.Cryptography {
    /// <remarks>
    /// This class is not thread-safe
    /// </remarks>
    public class SymmetricAlgorithmCipher : ICipher {
        private readonly SymmetricAlgorithm _algorithm;
        private readonly bool _disposeAlgorithm;

        private ICryptoTransform _encryptor;
        private ICryptoTransform _decryptor;

        public SymmetricAlgorithmCipher(SymmetricAlgorithm algorithm, bool disposeAlgorithm = true) {
            _algorithm = algorithm;
            _disposeAlgorithm = disposeAlgorithm;
        }

        public Stream CreateEncryptionStream(Stream target, CryptoStreamMode mode, bool leaveOpen = true) {
            var encryptor = _encryptor;

            if (encryptor == null) {
                encryptor = _algorithm.CreateEncryptor();
                if (encryptor.CanReuseTransform)
                    _encryptor = encryptor;
            }

            return new ExtendedCryptoStream(target, encryptor, mode, leaveOpen, !encryptor.CanReuseTransform);
        }

        public Stream CreateDecryptionStream(Stream target, CryptoStreamMode mode, bool leaveOpen = true) {
            var decryptor = _decryptor;

            if (decryptor == null) {
                decryptor = _algorithm.CreateDecryptor();
                if (decryptor.CanReuseTransform)
                    _decryptor = decryptor;
            }

            return new ExtendedCryptoStream(target, decryptor, mode, leaveOpen, !decryptor.CanReuseTransform);
        }

        public void Dispose() {
            _encryptor?.Dispose();
            _decryptor?.Dispose();

            if (_disposeAlgorithm)
                _algorithm.Dispose();
        }
    }
}