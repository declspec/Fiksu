using Omnis;
using Omnis.Auth;
using Omnis.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OmnisCore.Auth.Web {
    public class PassiveAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
        private readonly IExecutionEnvironment _environment;
        private readonly IEnumerable<IPassiveAuthenticationProvider<IHttpContext>> _providers;

        public PassiveAuthenticationHandler(IExecutionEnvironment environment, IEnumerable<IPassiveAuthenticationProvider<IHttpContext>> providers, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) {
            _environment = environment;
            _providers = providers;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            AuthenticationResult error = null;

            if (_providers != null) {
                var context = Context.RequestServices.GetRequiredService<IHttpContext>();

                foreach (var provider in _providers) {
                    var current = await provider.AuthenticateAsync(context, _environment);

                    if (current?.Successful == true) {
                        var principal = new ClaimsPrincipal(current.Identity);
                        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
                    }
                    else if (current != null && !current.Skipped) {
                        error = current;
                    }
                }
            }

            return error != null
                ? AuthenticateResult.Fail(error.Errors[0])
                : AuthenticateResult.NoResult();
        }
    }
}
