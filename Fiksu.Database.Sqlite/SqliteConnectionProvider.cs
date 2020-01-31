using Fiksu.Threading;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Fiksu.Database.Sqlite {
    public class SqliteConnectionProvider : IDbConnectionProvider {
        private const int DefaultPoolSize = 10;

        private readonly AsyncValuePool<IDbConnection> _pool;

        public SqliteConnectionProvider(string connectionString)
            : this(connectionString, DefaultPoolSize) { }

        public SqliteConnectionProvider(string connectionString, int poolSize) {
            if (poolSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(poolSize), "expected be a positive, non-zero integer");

            _pool = new AsyncValuePool<IDbConnection>(poolSize, _ => {
                var connection = new InternalSqliteConnection(connectionString, this);
                connection.Open();
                return connection;
            });
        }

        public Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken) {
            return _pool.AcquireAsync(cancellationToken);
        }

        private void ReturnConnectionToPool(InternalSqliteConnection connection) {
            _pool.Release(connection);
        }

        private class InternalSqliteConnection : SqliteConnection {
            private readonly SqliteConnectionProvider _owner;

            public InternalSqliteConnection(string connectionString, SqliteConnectionProvider owner) : base(connectionString) {
                _owner = owner;
            }

            public override void Close() {
                _owner.ReturnConnectionToPool(this);
            }
        }
    }
}
