using Fiksu.Web;
using System;
using System.Security.Principal;
using System.Web;

namespace FiksuClassic.Web.Internal
{
    public class NetClassicHttpContext : IHttpContext
    {
        private readonly HttpContextBase _context;

        private IHttpRequest _request;
        private IHttpResponse _response;

        public IHttpRequest Request
        {
            get => _request ?? (_request = new NetClassicHttpRequest(_context.Request));
        }

        public IHttpResponse Response
        {
            get => _response ?? (_response = new NetClassicHttpResponse(_context.Response));
        }
        public IPrincipal User
        {
            get => _context.User;
            set => _context.User = value;
        }

        public NetClassicHttpContext(HttpContextBase context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
