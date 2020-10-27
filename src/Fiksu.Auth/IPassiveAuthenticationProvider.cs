using System.Threading.Tasks;

namespace Fiksu.Auth {
    /// <summary>
    /// An authentication provider that can authenticate an existing context without any additional input.
    /// Examples of such a provider might be Cookie Authentication against an existing HTTP context.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IPassiveAuthenticationProvider<TContext> {
        /// <summary>
        /// Attempt to passively authenticate a context for a specific environment
        /// </summary>
        /// <param name="context">The current context to authenticate</param>
        /// <param name="environment">The environment against which authentication should be attempted</param>
        /// <returns>
        /// A successful AuthenticationResult with an identity describing the user if the context's session was valid,
        /// see <see cref="AuthenticationResult" />'s static members for common error results
        /// </returns>
        Task<AuthenticationResult> AuthenticateAsync(TContext context, IExecutionEnvironment environment);
    }
}
