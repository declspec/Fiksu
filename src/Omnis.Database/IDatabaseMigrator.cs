namespace Omnis.Database {
    public interface IDatabaseMigrator {
        void MigrateTo(long version);
        void MigrateToLatest();
    }
}
