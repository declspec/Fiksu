using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiksu.Azure {
    public interface IAzureTable<TEntity> where TEntity : class, ITableEntity, new() {
        Task CreateIfNotExistsAsync();
        Task<TEntity> GetEntityAsync(string partitionKey, string rowKey);
        Task<IList<TEntity>> GetPartitionAsync(string partitionKey);
        Task<IList<TEntity>> GetEntitiesAsync(TableQuery<TEntity> query);
        Task<IList<TEntity>> GetAllEntitiesAsync();
        Task CreateEntityAsync(TEntity entity);
        Task CreateOrUpdateEntityAsync(TEntity entity);
        Task DeleteEntityAsync(TEntity entity);
    }

    public class AzureTable<TEntity> : IAzureTable<TEntity> where TEntity : class, ITableEntity, new() {
        private const int EntityNotFoundStatusCode = 404;

        private readonly CloudTable _table;

        public AzureTable(CloudTable table) {
            _table = table;
        }

        public Task CreateIfNotExistsAsync() {
            return _table.CreateIfNotExistsAsync();
        }

        public Task<IList<TEntity>> GetAllEntitiesAsync() {
            return GetEntitiesAsync(new TableQuery<TEntity>());
        }

        public async Task<IList<TEntity>> GetEntitiesAsync(TableQuery<TEntity> query) {
            TableContinuationToken continuation = null;
            var results = new List<TEntity>();

            do {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, continuation).ConfigureAwait(false);
                continuation = segment.ContinuationToken;
                results.AddRange(segment.Results);
            } while (continuation != null);

            return results;
        }

        public async Task<TEntity> GetEntityAsync(string partitionKey, string rowKey) {
            try {
                var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
                var result = await _table.ExecuteAsync(operation).ConfigureAwait(false);
                return (TEntity)result.Result;
            }
            catch (StorageException ex) {
                if (ex.RequestInformation.HttpStatusCode != EntityNotFoundStatusCode)
                    throw;
                return null;
            }
        }

        public async Task<IList<TEntity>> GetPartitionAsync(string partitionKey) {
            return await GetEntitiesAsync(new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey)));
        }

        public Task CreateEntityAsync(TEntity entity) {
            var operation = TableOperation.Insert(entity);
            return _table.ExecuteAsync(operation);
        }

        public Task CreateOrUpdateEntityAsync(TEntity entity) {
            var operation = TableOperation.InsertOrReplace(entity);
            return _table.ExecuteAsync(operation);
        }

        public Task DeleteEntityAsync(TEntity entity) {
            var operation = TableOperation.Delete(entity);
            return _table.ExecuteAsync(operation);
        }
    }
}
