using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using OmnisClassic.Web.Http.Extensions;

namespace OmnisClassic.Web.Http {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeRequestAttribute : AuthorizationFilterAttribute {
        private readonly IList<string> _roles;

        public AuthorizeRequestAttribute(params string[] roles) {
            _roles = roles;
        }

        public override void OnAuthorization(HttpActionContext actionContext) {
            if (!ShouldSkipAuthorization(actionContext)) {
                if (!IsAuthenticated(actionContext))
                    actionContext.Response = actionContext.Request.Unauthorized();
                else if (!IsAuthorized(actionContext))
                    actionContext.Response = actionContext.Request.Forbidden();
            }
        }

        protected virtual bool IsAuthenticated(HttpActionContext actionContext) {
            var principal = actionContext.ControllerContext.RequestContext.Principal;
            return principal?.Identity?.IsAuthenticated == true;
        }

        protected virtual bool IsAuthorized(HttpActionContext actionContext) {
            // Guaranteed to be authenticated at this point
            return (_roles == null || _roles.Count == 0)
                || _roles.Any(actionContext.ControllerContext.RequestContext.Principal.IsInRole);
        }

        protected virtual bool ShouldSkipAuthorization(HttpActionContext actionContext) {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}