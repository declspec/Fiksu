using Omnis.Threading;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Omnis.Database.Sqlite {
    public class SqliteConnectionProvider : IDbConnectionProvider {
        private const int DefaultPoolSize = 10;

        private readonly AsyncObjectPool<IDbConnection> _pool;
        private readonly PooledObject<IDbConnection>[] _objects;

        public SqliteConnectionProvider(string connectionString)
            : this(connectionString, DefaultPoolSize) { }

        public SqliteConnectionProvider(string connectionString, int poolSize) {
            if (poolSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(poolSize), "expected to be a positive, non-zero integer");

            // Keep a parallel set of objects to be able to maintain the IDbConnectionProvider
            // interface.
            _objects = new PooledObject<IDbConnection>[poolSize];

            _pool = new AsyncObjectPool<IDbConnection>(poolSize, index => {
                var connection = new InternalSqliteConnection(connectionString, this, index);
                connection.Open();
                return connection;
            });
        }

        public async Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken) {
            var obj = await _pool.AcquireAsync(cancellationToken).ConfigureAwait(false);
            _objects[obj.Index] = obj;
            return obj.Value;
        }

        private void ReturnConnectionToPool(IDbConnection value, int poolIndex) {
            // Make sure no funny business with ref'd values being used after returned
            if (_objects[poolIndex].Value == value) {
                _objects[poolIndex].Dispose();
                _objects[poolIndex] = default;
            }
        }

        private void RemoveConnectionFromPool(IDbConnection value, int poolIndex) {
            // Make sure no funny business with ref'd values being used after returned
            if (_objects[poolIndex].Value == value) {
                _objects[poolIndex].Destroy();
                _objects[poolIndex] = default;
            }
        }

        private class InternalSqliteConnection : SqliteConnection {
            private readonly SqliteConnectionProvider _owner;
            private readonly int _index;

            public InternalSqliteConnection(string connectionString, SqliteConnectionProvider owner, int index) : base(connectionString) {
                _owner = owner;
                _index = index;
            }

            public override void Close() {
                _owner.ReturnConnectionToPool(this, _index);
            }

            protected override void Dispose(bool disposing) {
                base.Dispose(disposing);
                _owner.RemoveConnectionFromPool(this, _index);
            }
        }
    }
}
