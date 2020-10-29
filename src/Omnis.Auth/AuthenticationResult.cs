using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Omnis.Auth {
    /// <summary>
    /// Wrapper for an Authentication result, which can be either successful or unsuccessful,
    /// successful results should contain an authenticated identity, unsuccessful results should specify errors.
    /// </summary>
    public class AuthenticationResult {
        /// <summary>
        /// An error AuthenticationResult indicating bad credentials.
        /// </summary>
        public static readonly AuthenticationResult InvalidCredentials = Failure("Invalid username or password.");
        /// <summary>
        /// An error AuthenticationResult indicating that the underlying service is unavailable.
        /// </summary>
        public static readonly AuthenticationResult ServiceUnavailable = Failure("The authentication service is currently unavailable. Please try again later.");
        /// <summary>
        /// An error AuthenticationResult indicating that the target masquerade user is invalid.
        /// </summary>
        public static readonly AuthenticationResult InvalidMasqueradeTarget = Failure("Invalid masquerade target.");
        /// <summary>
        /// An error AuthenticationResult indicating that the underlying session has expired (i.e. a remote session).
        /// </summary>
        public static readonly AuthenticationResult SessionExpired = Failure("Session expired.");
        /// <summary>
        /// An error AuthenticationResult indicating that the user does not have access to the current operating (i.e. masquerade).
        /// </summary>
        public static readonly AuthenticationResult AccessDenied = Failure("Access denied.");
        /// <summary>
        /// An neutral (unsuccessful) AuthenticationResult indicating that no action was taken (i.e. feature is turned off, etc.)
        /// </summary>
        public static readonly AuthenticationResult Skip = Failure(default(IList<string>));

        public bool Successful { get; }
        public ClaimsIdentity Identity { get; }
        public IList<string> Errors { get; }
        public bool Skipped => !Successful && (Errors == null || Errors.Count == 0);

        protected AuthenticationResult(ClaimsIdentity identity) {
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Successful = true;
        }

        protected AuthenticationResult(IList<string> errors) {
            Errors = errors;
            Successful = false;
        }

        public static AuthenticationResult Success(ClaimsIdentity identity) {
            return new AuthenticationResult(identity);
        }

        public static AuthenticationResult Failure(IList<string> errors) {
            return new AuthenticationResult(errors);
        }

        public static AuthenticationResult Failure(string error) {
            return new AuthenticationResult(new[] { error });
        }
    }
}
