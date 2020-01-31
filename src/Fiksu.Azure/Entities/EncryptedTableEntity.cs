using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Fiksu.Logging;

namespace Fiksu.Azure.Entities {
    public class EncryptedTableEntity : TableEntity {
        public const int DefaultEncryptionVersion = 1;
        // No option for dependency injection here
        protected static readonly ILogger Logger = LoggerFactory.GetLogger(nameof(EncryptedTableEntity));

        public int? EncryptionVersion { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext) {
            base.ReadEntity(properties, operationContext);
            if (EncryptionVersion > 0)
                DecryptEntity(this);
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext) {
            if (!EncryptionVersion.HasValue)
                EncryptionVersion = DefaultEncryptionVersion;

            var properties = base.WriteEntity(operationContext);
            if (EncryptionVersion > 0)
                EncryptEntity(this, properties);
            return properties;
        }

        protected static void DecryptEntity(EncryptedTableEntity entity) {

            var algorithm = AzureEncryption.GetAlgorithm(entity.EncryptionVersion.Value);

            foreach (var info in GetEncyptableProperties(entity)) {
                try {
                    if (info.PropertyType == typeof(string)) {
                        var encrypted = (string)info.GetValue(entity);
                        if (!string.IsNullOrEmpty(encrypted)) {
                            var decrypted = Decrypt(Convert.FromBase64String(encrypted), algorithm);
                            info.SetValue(entity, Encoding.UTF8.GetString(decrypted));
                        }
                    }
                    else if (info.PropertyType == typeof(byte[])) {
                        var encrypted = (byte[])info.GetValue(entity);
                        if (encrypted != null && encrypted.Length > 0)
                            info.SetValue(entity, Decrypt(encrypted, algorithm));
                    }
                    else {
                        // TODO: Log, throw, what? I feel there should be some feedback if you mark an unsupported property as [Encrypt]
                    }
                }
                catch (Exception ex) {
                    Logger.Warn(ex, "Failed to decrypt {0}.{1}", entity.GetType().FullName, info.Name);
                }
            }
        }

        protected static void EncryptEntity(EncryptedTableEntity entity, IDictionary<string, EntityProperty> entityProperties) {

            var algorithm = AzureEncryption.GetAlgorithm(entity.EncryptionVersion.Value);

            foreach (var info in GetEncyptableProperties(entity)) {
                try {
                    if (!entityProperties.TryGetValue(info.Name, out var entityProp))
                        continue; // Do we need to log this, idk if it's every actually going to come up.

                    switch (entityProp.PropertyType) {
                        case EdmType.Binary:
                            if (entityProp.BinaryValue != null && entityProp.BinaryValue.Length > 0)
                                entityProp.BinaryValue = Encrypt(entityProp.BinaryValue, algorithm);
                            break;
                        case EdmType.String:
                            if (!string.IsNullOrEmpty(entityProp.StringValue))
                                entityProp.StringValue = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(entityProp.StringValue), algorithm));
                            break;
                        default:
                            // TODO: Log, throw, what? I feel there should be some feedback if you mark an unsupported property as [Encrypt]
                            break;
                    }
                }
                catch (Exception ex) {
                    Logger.Warn(ex, "Failed to encrypt {0}.{1}", entity.GetType().FullName, info.Name);
                }
            }
        }

        protected static byte[] Decrypt(byte[] data, SymmetricAlgorithm algorithm) {
            using (var transform = algorithm.CreateDecryptor())
            using (var decrypted = new MemoryStream()) {
                using (var crypto = new CryptoStream(decrypted, transform, CryptoStreamMode.Write))
                    crypto.Write(data, 0, data.Length);

                return decrypted.ToArray();
            }
        }

        protected static byte[] Encrypt(byte[] data, SymmetricAlgorithm algorithm) {
            using (var transform = algorithm.CreateEncryptor())
            using (var encrypted = new MemoryStream()) {
                using (var crypto = new CryptoStream(encrypted, transform, CryptoStreamMode.Write))
                    crypto.Write(data, 0, data.Length);

                return encrypted.ToArray();
            }
        }

        private static IEnumerable<PropertyInfo> GetEncyptableProperties(object obj) {
            return obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.GetCustomAttributes(typeof(EncryptAttribute), false).Length > 0);
        }
    }
}
