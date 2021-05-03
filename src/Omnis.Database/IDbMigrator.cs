using System.Data;

namespace Omnis.Database {
    public interface IDbMigrator {
        /// <summary>
        /// Migrate the database up or down to a particular version
        /// </summary>
        /// <param name="connection">A connection to the database to be migrated</param>
        /// <param name="version">The desired version to migrate to</param>
        /// <returns>The total number of migrations applied or rolled back (negative value for rollbacks)</returns>
        int MigrateTo(IDbConnection connection, long version);
        /// <summary>
        /// Migrate the database up to the latest version
        /// </summary>
        /// <param name="connection">A connection to the database to be migrated</param>
        /// <returns>The total number of migrations applied</returns>
        int MigrateToLatest(IDbConnection connection);
    }
}
