using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fiksu.Auth.Identity
{
    /// <summary>
    /// Claim provider that transforms the results of IRoleProviders
    /// into role claims
    /// </summary>
    public class RoleClaimsProvider : IClaimsProvider
    {
        private readonly IEnumerable<IRoleProvider> _roleProviders;

        public RoleClaimsProvider(IEnumerable<IRoleProvider> roleProviders)
        {
            _roleProviders = roleProviders;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(ClaimsIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            var claimType = string.IsNullOrEmpty(identity.RoleClaimType) 
                ? ClaimsIdentity.DefaultRoleClaimType 
                : identity.RoleClaimType;

            var claims = new List<Claim>();

            foreach(var provider in _roleProviders)
            {
                var providerId = provider.GetType().Name;

                try
                {
                    var roles = await provider.GetRolesForUserAsync(identity.Name).ConfigureAwait(false);
                    claims.AddRange(roles.Select(r => new Claim(claimType, r, ClaimValueTypes.String, providerId)));
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("Role provider {0} failed for {1}", providerId, identity.Name), ex);
                }
            }

            return claims;
        }
    }
}
