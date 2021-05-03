using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Omnis.Database {
    /// <summary>
    /// A reusable source of database connections
    /// </summary>
    public interface IDbConnectionProvider {
        /// <summary>
        /// Obtain a connection to the database
        /// </summary>
        /// <param name="cancellationToken">A cancellation token used to cancel the connection request</param>
        /// <returns>A valid connection to the database</returns>
        /// <remarks>The returned connection is owned by the caller and must be disposed of correctly.</remarks>
        Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
