﻿using System.Threading.Tasks;

namespace Fiksu.Auth
{
    /// <summary>
    /// An active authentication (username/password) provider
    /// </summary>
    public interface IActiveAuthenticationProvider
    {
        /// <summary>
        /// Attempt to authenticate a username/password combination against a particular environment.
        /// </summary>
        /// <param name="userName">The identifier of the user being authenticated</param>
        /// <param name="password">The password of the user being authenticated</param>
        /// <param name="environment">The target environment to authenticate against</param>
        /// <returns>An Task while resolves to result of the authentication request, see <see cref="AuthenticationResult"/>'s static members for predefined results</returns>
        Task<AuthenticationResult> AuthenticateAsync(string userName, string password, ExecutionEnvironment environment);
    }
}
