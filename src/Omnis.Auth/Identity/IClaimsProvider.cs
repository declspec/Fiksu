﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Omnis.Auth.Identity {
    /// <summary>
    /// Provides a set of claims
    /// </summary>
    public interface IClaimsProvider {
        /// <summary>
        /// Gets a set of claims from this provider for an identity (does not modify the identity)
        /// </summary>
        /// <param name="identity">Identity of the user whose claims are to be looked up</param>
        /// <returns>A set of claims returned by this provider</returns>
        Task<IEnumerable<Claim>> GetClaimsAsync(ClaimsIdentity identity);
    }
}
