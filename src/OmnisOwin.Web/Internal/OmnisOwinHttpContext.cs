using Omnis.Web;
using System;
using System.Security.Principal;
using Microsoft.Owin;
using System.Security.Claims;

namespace OmnisClassic.Web.Internal {
    public class OmnisOwinHttpContext : IHttpContext {
        private readonly IOwinContext _context;

        private IHttpRequest _request;
        private IHttpResponse _response;

        public IHttpRequest Request {
            get => _request ?? (_request = new OmnisOwinHttpRequest(_context.Request));
        }

        public IHttpResponse Response {
            get => _response ?? (_response = new OmnisOwinHttpResponse(_context.Response));
        }
        public IPrincipal User {
            get => _context.Authentication?.User;
            set => _context.Authentication.User = (ClaimsPrincipal)value;
        }

        public OmnisOwinHttpContext(IOwinContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
