using Omnis.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace OmnisCore.Web.Internal {
    public class OmnisCoreHttpContext : IHttpContext {
        private readonly HttpContext _context;
        private IHttpRequest _request;
        private IHttpResponse _response;

        public IPrincipal User {
            get => _context.User;
            set => _context.User = (ClaimsPrincipal)value;
        }

        public IHttpRequest Request {
            get { return _request ?? (_request = new OmnisCoreHttpRequest(_context.Request)); }
        }

        public IHttpResponse Response {
            get { return _response ?? (_response = new OmnisCoreHttpResponse(_context.Response)); }
        }

        public OmnisCoreHttpContext(HttpContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
