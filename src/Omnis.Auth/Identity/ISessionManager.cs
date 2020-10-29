using System.Security.Claims;
using System.Threading.Tasks;

namespace Omnis.Auth.Identity {
    /// <summary>
    /// Handles the starting/restoring/ending of identity sessions.
    /// </summary>
    public interface ISessionManager<TContext> {
        /// <summary>
        /// Sign a principal into an Identity Session.
        /// </summary>
        /// <param name="principal">The principal to sign in.</param>
        /// <param name="context">Current context</param>
        Task SignInAsync(ClaimsPrincipal principal, TContext context);

        /// <summary>
        /// Attempts to restore an existing Identity Session, if one exists.
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns>True if a session was restored, false otherwise.</returns>
        Task<bool> TryRestoreAsync(TContext context);

        /// <summary>
        /// Ends an Identity Session
        /// </summary>
        /// <param name="context">Current context</param>
        Task SignOutAsync(TContext context);
    }
}
