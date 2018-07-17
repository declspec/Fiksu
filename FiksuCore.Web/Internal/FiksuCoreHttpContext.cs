using Fiksu.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace FiksuCore.Web.Internal
{
    public class FiksuCoreHttpContext : IHttpContext
    {
        private readonly HttpContext _context;
        private IHttpRequest _request;
        private IHttpResponse _response;

        public IPrincipal User
        {
            get => _context.User;
            set => _context.User = (ClaimsPrincipal)value;
        }

        public IHttpRequest Request
        {
            get { return _request ?? (_request = new FiksuCoreHttpRequest(_context.Request)); }
        }

        public IHttpResponse Response
        {
            get { return _response ?? (_response = new FiksuCoreHttpResponse(_context.Response)); }
        }

        public FiksuCoreHttpContext(HttpContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
