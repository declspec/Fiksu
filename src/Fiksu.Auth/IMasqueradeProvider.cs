using System.Security.Claims;
using System.Threading.Tasks;

namespace Fiksu.Auth {
    /// <summary>
    /// Interface for creating a new masqueraded identity based on a target user
    /// </summary>
    /// <remarks>
    /// Unlike the old system, authentication of the original user is not performed 
    /// here, it should have been done previously. (i.e. by an <see cref="IBasicAuthenticationProvider"/>)
    /// </remarks>
    public interface IMasqueradeProvider {
        /// <summary>
        /// Attempt to create a masqueraded identity for a target user based on an authenticated principal
        /// </summary>
        /// <param name="currentPrincipal">An authenticated principal requesting the masqueraded identity</param>
        /// <param name="targerUserName">The identifier of the target user to masquerade as</param>
        /// <returns>A success or failure AuthenticationResult</returns>
        Task<AuthenticationResult> MasqueradeAsync(ClaimsPrincipal currentPrincipal, string targerUserName);
    }
}
