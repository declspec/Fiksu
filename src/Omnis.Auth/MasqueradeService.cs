using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Omnis.Auth.Extensions;

namespace Omnis.Auth {
    /// <summary>
    /// Service to aggregate and provide security over <see cref="IMasqueradeProvider"/> instances
    /// </summary>
    public interface IMasqueradeService {
        /// <summary>Attempt to retrieve a masqueraded identity for a target user name</summary>
        /// <param name="currentPrincipal">The principal attempting to masquerade</param>
        /// <param name="targetUserName">The name of the user to obtain a masqueraded identity from</param>
        /// <returns>
        /// <see cref="AuthenticationResult.Skip"/> if no Masquerade providers have been registered, or if all return <see cref="AuthenticationResult.Skip"/>,
        /// <see cref="AuthenticationResult.InvalidMasqueradeTarget"/> if <paramref name="targetUserName"/> is invalid,
        /// An <see cref="AuthenticationResult"/> containing the masqueraded identity if one of the providers was successful or
        /// an <see cref="AuthenticationResult"/> containing all the distinct errors from all registered providers if none are successful
        /// </returns>
        Task<AuthenticationResult> MasqueradeAsync(ClaimsPrincipal currentPrincipal, string targetUserName);
    }

    public class MasqueradeService : IMasqueradeService {
        private readonly IEnumerable<IMasqueradeProvider> _masqueradeProviders;

        private readonly IList<string> _masqueradeRoles;
        private readonly IList<string> _restrictedMasqueradeRoles;

        public MasqueradeService(IEnumerable<IMasqueradeProvider> masqueradeProviders, IList<string> masqueradeRoles, IList<string> restrictedMasqueradeRoles = null) {
            _masqueradeProviders = masqueradeProviders;

            _masqueradeRoles = masqueradeRoles;
            _restrictedMasqueradeRoles = restrictedMasqueradeRoles;
        }

        public async Task<AuthenticationResult> MasqueradeAsync(ClaimsPrincipal principal, string targetUserName) {
            if (principal == null)
                throw new ArgumentNullException("principal");

            if (_masqueradeProviders == null || !_masqueradeProviders.Any())
                return AuthenticationResult.Failure("No masquerade providers configured in the application.");

            if (!HasMasqueradePermission(principal))
                return AuthenticationResult.AccessDenied;

            if (string.IsNullOrEmpty(targetUserName))
                return AuthenticationResult.InvalidMasqueradeTarget;

            var result = await _masqueradeProviders.AggregateResultsAsync(provider => provider.MasqueradeAsync(principal, targetUserName))
                .ConfigureAwait(false);

            if (result.Successful && !IsAccessibleMasqueradeTarget(principal, result.Identity))
                result = AuthenticationResult.Failure("Insufficient privileges to masquerade as target user.");

            return result;
        }

        private bool HasMasqueradePermission(ClaimsPrincipal principal) {
            // Has permission when masqueradeRoles is explicitly set to empty, 
            // or if the authentication service roles contains an approved role, 
            // or any role providers return an approved role for the user
            var principalRoles = principal.GetRoles();
            return _masqueradeRoles != null && (_masqueradeRoles.Count == 0 || _masqueradeRoles.Any(role => principalRoles.Contains(role, StringComparer.OrdinalIgnoreCase)));
        }

        private bool IsAccessibleMasqueradeTarget(ClaimsPrincipal principal, ClaimsIdentity targetIdentity) {
            if (_restrictedMasqueradeRoles == null)
                return true;

            // Ensure that the real user has similar permission levels to the masquerade target.
            // Stops users from elevating their own access by masquerading as a target with more permissions.
            return !targetIdentity.GetRoles().Except(principal.GetRoles(), StringComparer.OrdinalIgnoreCase)
                .Any(role => _restrictedMasqueradeRoles.Contains(role, StringComparer.OrdinalIgnoreCase));
        }
    }
}
