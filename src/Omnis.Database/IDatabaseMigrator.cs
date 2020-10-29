namespace Omnis.Database {
    public interface IDatabaseMigrator {
        /// <summary>
        /// Migrate the database up or down to a particular version
        /// </summary>
        /// <param name="version">The desired version to migrate to</param>
        /// <returns>The total number of migrations applied or rolled back (negative value for rollbacks)</returns>
        int MigrateTo(long version);
        /// <summary>
        /// Migrate the database up to the latest version
        /// </summary>
        /// <returns>The total number of migrations applied</returns>
        int MigrateToLatest();
    }
}
