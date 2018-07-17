using System.Data;

namespace Fiksu.Database {
    public interface IDbContext {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
    }

    public class DbContext : IDbContext {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbContext(IDbConnection connection) {

        }
    }
}
