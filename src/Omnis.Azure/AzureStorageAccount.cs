using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Omnis.Azure {
    public interface IAzureStorageAccount {
        IAzureTable<TEntity> GetTableReference<TEntity>() where TEntity : class, ITableEntity, new();
        IAzureTable<TEntity> GetTableReference<TEntity>(string tableName) where TEntity : class, ITableEntity, new();
    }

    public class AzureStorageAccount : IAzureStorageAccount {
        private readonly CloudStorageAccount _account;

        public AzureStorageAccount(CloudStorageAccount account) {
            _account = account;
        }

        public IAzureTable<TEntity> GetTableReference<TEntity>() where TEntity : class, ITableEntity, new() {
            return GetTableReference<TEntity>(typeof(TEntity).Name);
        }

        public IAzureTable<TEntity> GetTableReference<TEntity>(string tableName) where TEntity : class, ITableEntity, new() {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var client = _account.CreateCloudTableClient();
            return new AzureTable<TEntity>(client.GetTableReference(tableName));
        }
    }
}