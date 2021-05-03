using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Omnis.Database.Sqlite {
    public class SqliteDbMigrator : IDbMigrator {
        private static readonly Type MigrationType = typeof(IMigration);

        private const string CreateVersionInfoTableDdl =
            @"CREATE TABLE IF NOT EXISTS version_info (
                version         INTEGER PRIMARY KEY,
                title           TEXT NOT NULL,
                date_created    TIMESTAMP NOT NULL
            ) WITHOUT ROWID";

        private readonly IReadOnlyList<Assembly> _migrationAssemblies;

        public SqliteDbMigrator(params Assembly[] migrationAssemblies) {
            if (migrationAssemblies == null)
                throw new ArgumentNullException(nameof(migrationAssemblies));

            if (migrationAssemblies.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(migrationAssemblies), "Must specify at least one assembly to retrieve migrations from");

            _migrationAssemblies = migrationAssemblies;
        }

        public int MigrateToLatest(IDbConnection connection) {
            return MigrateTo(connection, long.MaxValue);
        }

        public int MigrateTo(IDbConnection connection, long version) {
            // First, ensure the table exists
            connection.Execute(CreateVersionInfoTableDdl);
            var latest = connection.ExecuteScalar<long>("SELECT MAX(version) FROM version_info");

            return (version > latest)
                ? MigrateUpTo(connection, version, latest)
                : -MigrateDownTo(connection, version);
        }

        private int MigrateUpTo(IDbConnection connection, long version, long from) {
            var migrationType = typeof(IMigration);
            var completed = new Stack<MigrationOperation>();

            var migrations = FindMetadata()
                .Where(m => m.Attribute.Version > from && m.Attribute.Version < version)
                .OrderBy(m => m.Attribute.Version);

            try {
                foreach (var m in migrations) {
                    var migration = (IMigration)Activator.CreateInstance(m.Type);
                    var operation = new MigrationOperation(migration, m.Attribute.Version, m.Attribute.Title ?? m.Type.Name);

                    migration.Up(connection);
                    completed.Push(operation);
                    connection.Execute("INSERT INTO version_info(version,title,date_created) VALUES(@version,@title,current_timestamp)", new { version = operation.Version, title = operation.Title });
                }
            }
            catch {
                // Attempt to rollback
                Rollback(connection, completed);
                throw; // Bubble the exception
            }

            return completed.Count;
        }

        private int MigrateDownTo(IDbConnection connection, long version) {
            var migrationType = typeof(IMigration);
            var completed = new Stack<MigrationOperation>();

            var todo = connection.Query<long>("SELECT version FROM version_info WHERE version > @Version", new { Version = version });

            var migrations = FindMetadata()
                .Where(m => todo.Contains(m.Attribute.Version))
                .OrderByDescending(m => m.Attribute.Version);

            try {
                foreach (var m in migrations) {
                    var migration = (IMigration)Activator.CreateInstance(m.Type);
                    var operation = new MigrationOperation(migration, m.Attribute.Version, m.Attribute.Title ?? m.Type.Name);

                    migration.Down(connection);
                    completed.Push(operation);
                    connection.Execute("DELETE FROM version_info WHERE version = @Version", new { Version = operation.Version });
                }
            }
            catch {
                // Attempt to rollback
                Rollup(connection, completed);
                throw; // Bubble the exception
            }

            return completed.Count;
        }

        private void Rollback(IDbConnection connection, Stack<MigrationOperation> migrations) {
            var lastSuccessful = long.MaxValue;

            try {
                while (migrations.Count > 0) {
                    var current = migrations.Pop();
                    current.Migration.Down(connection);
                    lastSuccessful = current.Version;
                }
            }
            finally {
                // Even an exception occurs during one of the 'Down' migrations, ensure that the 'version_info' table is up-to-date
                connection.Execute("DELETE FROM version_info WHERE version >= @last", new { last = lastSuccessful });
            }
        }

        private void Rollup(IDbConnection connection, Stack<MigrationOperation> migrations) {
            while (migrations.Count > 0) {
                var current = migrations.Pop();
                current.Migration.Up(connection);
                connection.Execute("INSERT INTO version_info(version,title,date_created) VALUES(@version,@title,current_timestamp)", new { version = current.Version, title = current.Title });
            }
        }

        private IEnumerable<MigrationMetadata> FindMetadata() {
            return _migrationAssemblies.SelectMany(a => a.GetTypes().Where(MigrationType.IsAssignableFrom))
                .Select(t => new MigrationMetadata(t, t.GetTypeInfo().GetCustomAttribute<MigrationAttribute>()))
                .Where(m => m.Attribute != null);
        }

        private class MigrationMetadata {
            public Type Type { get; }
            public MigrationAttribute Attribute { get; }

            public MigrationMetadata(Type type, MigrationAttribute attribute) {
                Type = type;
                Attribute = attribute;
            }
        }

        private class MigrationOperation {
            public IMigration Migration { get; }
            public long Version { get; }
            public string Title { get; }

            public MigrationOperation(IMigration migration, long version, string title) {
                Migration = migration;
                Version = version;
                Title = title;
            }
        }
    }
}
