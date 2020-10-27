using System.Collections.Generic;
using System.Threading.Tasks;
using Fiksu.Auth.Extensions;

namespace Fiksu.Auth {
    /// <summary>
    /// A service for providing basic (username/password) authentication
    /// </summary>
    public interface IBasicAuthenticationService {
        /// <summary>
        /// Authenticate a user by their credentials against the current execution environment
        /// </summary>
        /// <param name="userName">The identifier of the user being authenticated</param>
        /// <param name="password">The password of the user being authenticated</param>
        /// <returns>
        /// An AuthenticationResult containing all the distinct errors from all registered providers if none are successful
        /// An AuthenticationResult containing the authenticated identity if one of the providers was successful
        /// </returns>
        /// <returns>An AuthenticationResult containing either an authenticated identity of the user, or a set of errors</returns>
        Task<AuthenticationResult> AuthenticateAsync(string userName, string password);

        /// <summary>
        /// Authenticate a user by their credentials against a target execution environment
        /// </summary>
        /// <param name="userName">The identifier of the user being authenticated</param>
        /// <param name="password">The password of the user being authenticated</param>
        /// <param name="environment">The target environment to authenticate against</param>
        /// <returns>
        /// An AuthenticationResult containing all the distinct errors from all registered providers if none are successful
        /// An AuthenticationResult containing the authenticated identity if one of the providers was successful
        /// </returns>
        /// <returns>An AuthenticationResult containing either an authenticated identity of the user, or a set of errors.</returns>
        Task<AuthenticationResult> AuthenticateAsync(string userName, string password, IExecutionEnvironment environment);
    }

    public class BasicAuthenticationService : IBasicAuthenticationService {
        private readonly IExecutionEnvironment _environment;
        private readonly IEnumerable<IBasicAuthenticationProvider> _authenticationProviders;

        public BasicAuthenticationService(IExecutionEnvironment environment, IEnumerable<IBasicAuthenticationProvider> authenticationProviders) {
            _environment = environment;
            _authenticationProviders = authenticationProviders;
        }

        public Task<AuthenticationResult> AuthenticateAsync(string userName, string password) {
            return AuthenticateAsync(userName, password, _environment);
        }

        public Task<AuthenticationResult> AuthenticateAsync(string userName, string password, IExecutionEnvironment environment) {
            return _authenticationProviders.AggregateResultsAsync(provider => provider.AuthenticateAsync(userName, password, environment));
        }
    }
}
