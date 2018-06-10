using Fiksu.Web;
using System;
using System.Security.Principal;
using System.Web;

namespace FiksuClassic.Web.Internal
{
    public class FiksuClassicHttpContext : IHttpContext
    {
        private readonly HttpContextBase _context;

        private IHttpRequest _request;
        private IHttpResponse _response;

        public IHttpRequest Request
        {
            get => _request ?? (_request = new FiksuClassicHttpRequest(_context.Request));
        }

        public IHttpResponse Response
        {
            get => _response ?? (_response = new FiksuClassicHttpResponse(_context.Response));
        }
        public IPrincipal User
        {
            get => _context.User;
            set => _context.User = value;
        }

        public FiksuClassicHttpContext(HttpContextBase context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
