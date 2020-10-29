using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Omnis.Auth.Identity;
using Omnis.Web;
using OmnisCore.Web.Internal;

namespace OmnisCore.Auth.Web.Middleware {
    public class SessionRestoreMiddleware {
        private readonly RequestDelegate _next;
        private readonly ISessionManager<IHttpContext> _sessionManager;

        public SessionRestoreMiddleware(RequestDelegate next, ISessionManager<IHttpContext> sessionManager) {
            _next = next;
            _sessionManager = sessionManager;
        }

        public async Task Invoke(HttpContext context) {
            if (!context.User.Identity.IsAuthenticated) {
                var ctx = context.RequestServices.GetService<IHttpContext>()
                    ?? new OmnisCoreHttpContext(context);

                await _sessionManager.TryRestoreAsync(ctx).ConfigureAwait(false);
            }

            await _next(context).ConfigureAwait(false);
        }
    }
}
