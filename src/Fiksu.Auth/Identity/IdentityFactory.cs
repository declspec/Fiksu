using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Fiksu.Auth.Identity {
    /// <summary>
    /// Handles common ways to create an identity.
    /// </summary>
    public interface IIdentityFactory {
        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="roles">A list of roles to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, params string[] roles);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="roles">A list of roles to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<string> roles);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="claims">A list of claims to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, params Claim[] claims);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="claims">A list of claims to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<Claim> claims);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="role">A role to attach to the identity</param>
        /// <param name="claims">A list of claims to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, string role, params Claim[] claims);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="role">A role to attach to the identity</param>
        /// <param name="claims">A list of claims to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, string role, IEnumerable<Claim> claims);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="roles">A list of roles to attach to the identity</param>
        /// <param name="claims">A list of claims to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<string> roles, params Claim[] claims);

        /// <summary>
        /// Create a user's identity based off their name for a particular authentication type 
        /// </summary>
        /// <param name="authenticationType">The authentication type used to create the identity</param>
        /// <param name="username">The unique identifier for the user's identity</param>
        /// <param name="roles">A list of roles to attach to the identity</param>
        /// <param name="claims">A list of claims to attach to the identity</param>
        /// <returns>Returns an identity for the user, including claims from any registered <see cref="IClaimsProvider"/>s</returns>
        Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<string> roles, IEnumerable<Claim> claims);

        /// <summary>
        /// Create a user's identity based off an existing identity, potentially from an external source.
        /// </summary>
        /// <param name="identity">An existing IIdentity instance</param>
        /// <returns>Returns a fully fleshed-out identity for the user, including claims from any registered <see cref="IClaimsProvider" />s</returns>
        /// <remarks>Use this method to clone an identity from an external source and attach any claims from the <see cref="IClaimsProvider" />s</remarks>
        Task<ClaimsIdentity> CreateAsync(IIdentity identity);
    }

    public class IdentityFactory : IIdentityFactory {
        private readonly IEnumerable<IClaimsProvider> _claimsProviders;

        public IdentityFactory(IEnumerable<IClaimsProvider> claimsProviders) {
            _claimsProviders = claimsProviders;
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username) {
            return CreateAsync(authenticationType, username, Enumerable.Empty<Claim>());
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, params string[] roles) {
            return CreateAsync(authenticationType, username, ToRoleClaims(roles));
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<string> roles) {
            return CreateAsync(authenticationType, username, ToRoleClaims(roles));
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, params Claim[] claims) {
            return CreateAsync(authenticationType, username, (IEnumerable<Claim>)claims);
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, string role, params Claim[] claims) {
            return CreateAsync(authenticationType, username, Enumerable.Repeat(role, 1), claims);
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, string role, IEnumerable<Claim> claims) {
            return CreateAsync(authenticationType, username, Enumerable.Repeat(role, 1), claims);
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<string> roles, params Claim[] claims) {
            return CreateAsync(authenticationType, username, roles, (IEnumerable<Claim>)claims);
        }

        public Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<string> roles, IEnumerable<Claim> claims) {
            return CreateAsync(authenticationType, username, ToRoleClaims(roles).Concat(claims));
        }

        public async Task<ClaimsIdentity> CreateAsync(string authenticationType, string username, IEnumerable<Claim> claims) {
            var identity = new ClaimsIdentity(authenticationType);
            identity.AddClaim(new Claim(identity.NameClaimType, username.ToUpper()));

            if (claims != null)
                identity.AddClaims(claims);

            await PopulateClaimsAsync(identity).ConfigureAwait(false);
            return identity;
        }

        public async Task<ClaimsIdentity> CreateAsync(IIdentity identity) {
            var claimsIdentity = new ClaimsIdentity(identity);
            await PopulateClaimsAsync(claimsIdentity).ConfigureAwait(false);
            return claimsIdentity;
        }

        private async Task PopulateClaimsAsync(ClaimsIdentity identity) {
            if (_claimsProviders != null) {
                var claimsTasks = _claimsProviders.Select(p => p.GetClaimsAsync(identity));
                var results = await Task.WhenAll(claimsTasks).ConfigureAwait(false);

                foreach (var result in results) {
                    if (result != null)
                        identity.AddClaims(result);
                }
            }
        }

        private static IEnumerable<Claim> ToRoleClaims(IEnumerable<string> roles) {
            if (roles == null)
                yield break;

            foreach (var role in roles) {
                if (!string.IsNullOrEmpty(role))
                    yield return new Claim(ClaimsIdentity.DefaultRoleClaimType, role);
            }
        }
    }
}
