using Fiksu.Web;
using Microsoft.AspNetCore.Http;
using System;

namespace FiksuCore.Web.Internal
{
    internal class FiksuCoreHttpRequest : IHttpRequest
    {
        private readonly HttpRequest _request;
        private IHttpQueryString _query;
        private IHttpRequestCookies _cookies;
        private IHttpHeaderDictionary _headers;

        public string ContentType => _request.ContentType;
        public long? ContentLength => _request.ContentLength;
        public string Method => _request.Method;
        public bool IsSecure => _request.IsHttps;
        public string Path => _request.PathBase + _request.Path;

        public IHttpHeaderDictionary Headers
        {
            get => _headers ?? (_headers = new FiksuCoreHttpHeaderDictionary(_request.Headers));
        }

        public IHttpQueryString QueryString
        {
            get => _query ?? (_query = new FiksuCoreHttpQueryString(_request.Query));
        }

        public IHttpRequestCookies Cookies
        {
            get => _cookies ?? (_cookies = new FiksuCoreHttpRequestCookies(_request.Cookies));
        }

        public FiksuCoreHttpRequest(HttpRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }
    }
}
