using Fiksu.Threading;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Fiksu.Database.Sqlite {
    public class SqliteConnectionProvider : IDbConnectionProvider {
        private const int DefaultPoolSize = 10;

        private readonly AsyncObjectPool<IDbConnection> _pool;

        public SqliteConnectionProvider(string connectionString)
            : this(connectionString, DefaultPoolSize) { }

        public SqliteConnectionProvider(string connectionString, int poolSize) {
            if (poolSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(poolSize), "expected be a positive, non-zero integer");

            var factory = new Func<int, IDbConnection>(_ => {
                var conn = new InternalSqliteConnection(_pool, connectionString);
                conn.Open();
                return conn;
            });

            _pool = new AsyncObjectPool<IDbConnection>(poolSize, factory, (_, conn) => ((InternalSqliteConnection)conn).Close());
        }

        public Task<IDbConnection> GetConnectionAsync() {
            return GetConnectionAsync(CancellationToken.None);
        }

        public Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken) {
            return _pool.AcquireAsync(cancellationToken);
        }

        private class InternalSqliteConnection : SqliteConnection {
            private readonly AsyncObjectPool<IDbConnection> _pool;

            public InternalSqliteConnection(AsyncObjectPool<IDbConnection> pool, string connectionString) : base(connectionString) {
                _pool = pool;
            }

            public override void Close() {
                Close(false);
            }

            internal void Close(bool finalizing) {
                if (finalizing)
                    base.Close();
                else
                    _pool.Release(this);
            }
        }
    }
}
