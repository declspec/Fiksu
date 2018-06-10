using Fiksu.Web;
using System;
using System.Security.Principal;
using Microsoft.Owin;
using System.Security.Claims;

namespace FiksuClassic.Web.Internal
{
    public class FiksuOwinHttpContext : IHttpContext
    {
        private readonly IOwinContext _context;

        private IHttpRequest _request;
        private IHttpResponse _response;

        public IHttpRequest Request
        {
            get => _request ?? (_request = new FiksuOwinHttpRequest(_context.Request));
        }

        public IHttpResponse Response
        {
            get => _response ?? (_response = new FiksuOwinHttpResponse(_context.Response));
        }
        public IPrincipal User
        {
            get => _context.Authentication?.User;
            set => _context.Authentication.User = (ClaimsPrincipal)value;
        }

        public FiksuOwinHttpContext(IOwinContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
