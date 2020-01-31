using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;

namespace Fiksu.Azure {
    public interface IAzureStorageAccountFactory {
        IAzureStorageAccount GetStorageAccount(string accountName);
        void RegisterStorageAccount(string accountName, string connectionString);
        void RegisterStorageAccount(string accountName, CloudStorageAccount account);
    }

    public class AzureStorageAccountFactory : IAzureStorageAccountFactory {
        private readonly IDictionary<string, IAzureStorageAccount> _accounts;

        public AzureStorageAccountFactory() {
            _accounts = new Dictionary<string, IAzureStorageAccount>();
        }

        public IAzureStorageAccount GetStorageAccount(string accountName) {
            return _accounts[accountName];
        }

        public void RegisterStorageAccount(string accountName, string connectionString) {
            if (string.IsNullOrEmpty(accountName))
                throw new ArgumentNullException(nameof(accountName));

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            var account = CloudStorageAccount.Parse(connectionString);
            _accounts.Add(accountName, new AzureStorageAccount(account));
        }

        public void RegisterStorageAccount(string accountName, CloudStorageAccount account) {
            if (string.IsNullOrEmpty(accountName))
                throw new ArgumentNullException(nameof(accountName));

            if (account == null)
                throw new ArgumentNullException(nameof(account));

            _accounts.Add(accountName, new AzureStorageAccount(account));
        }
    }
}