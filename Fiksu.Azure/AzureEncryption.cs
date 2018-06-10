using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Fiksu.Azure
{
    public static class AzureEncryption
    {
        private static IDictionary<int, SymmetricAlgorithm> _keyStore;

        public static void Initialise(IDictionary<int, SymmetricAlgorithm> encryptionVersions)
        {
            if (_keyStore != null)
                throw new InvalidOperationException(string.Format("{0} has already been initialised", nameof(AzureEncryption)));

            _keyStore = encryptionVersions ?? throw new ArgumentNullException(nameof(encryptionVersions));
        }
        
        public static SymmetricAlgorithm GetAlgorithm(int version)
        {
            if (_keyStore == null)
                throw new InvalidOperationException(string.Format("{0} has not been initialised", nameof(AzureEncryption)));

            return _keyStore[version];
        }
    }
}
