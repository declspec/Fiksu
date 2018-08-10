using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Fiksu.Cryptography {
    public static class X509CertificateLocator {
        public static IEnumerable<X509Certificate2> FindByThumbprint(string thumbprint, StoreName storeName = StoreName.My) {
            return FindByThumbprintInLocation(thumbprint, storeName, StoreLocation.CurrentUser)
                .Concat(FindByThumbprintInLocation(thumbprint, storeName, StoreLocation.LocalMachine));
        }

        public static IEnumerable<X509Certificate2> FindByThumbprintInLocation(string thumbprint, StoreName storeName, StoreLocation storeLocation) {
            if (string.IsNullOrEmpty(thumbprint))
                throw new ArgumentNullException(nameof(thumbprint));

            using (var store = new X509Store(storeName, storeLocation)) {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                return store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint.Replace(" ", ""), false)
                    .Cast<X509Certificate2>();
            }
        }
    }
}
