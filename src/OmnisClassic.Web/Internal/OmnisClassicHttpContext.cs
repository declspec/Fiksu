using Omnis.Web;
using System;
using System.Security.Principal;
using System.Web;

namespace OmnisClassic.Web.Internal {
    public class OmnisClassicHttpContext : IHttpContext {
        private readonly HttpContextBase _context;

        private IHttpRequest _request;
        private IHttpResponse _response;

        public IHttpRequest Request {
            get => _request ?? (_request = new OmnisClassicHttpRequest(_context.Request));
        }

        public IHttpResponse Response {
            get => _response ?? (_response = new OmnisClassicHttpResponse(_context.Response));
        }
        public IPrincipal User {
            get => _context.User;
            set => _context.User = value;
        }

        public OmnisClassicHttpContext(HttpContextBase context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
