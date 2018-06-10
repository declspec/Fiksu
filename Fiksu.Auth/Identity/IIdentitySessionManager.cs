using System.Security.Claims;
using System.Threading.Tasks;

namespace Fiksu.Auth.Identity
{
    /// <summary>
    /// Handles the starting/restoring/ending of identity sessions.
    /// </summary>
    /// <remarks>
    /// This is a temporary glue class to work with the old pre-middleware/identity management workflow.
    /// Implementations should be kept as basic as possible to allow for an easier change to allow easier transitions to a newer system.
    /// All of the methods will fire off matching <see cref="IdentitySessionEvent"/>s 
    /// </remarks>
    public interface IIdentitySessionManager<TContext>
    {
        /// <summary>
        /// Sign a principal into an Identity Session.
        /// </summary>
        /// <param name="principal">The principal to sign in.</param>
        /// <param name="rememberMe">Controls if the session should be persisted.</param>
        Task SignInAsync(ClaimsPrincipal principal, TContext context);

        /// <summary>
        /// Attempts to restore an existing Identity Session, if one exists.
        /// </summary>
        /// <returns>True if a session was restored, false otherwise.</returns>
        Task<bool> TryRestoreAsync(TContext context);

        /// <summary>
        /// Ends an Identity Session
        /// </summary>
        Task SignOutAsync(TContext context);
    }
}
