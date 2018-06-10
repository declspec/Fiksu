using Fiksu.Web;
using System;
using System.Web;

namespace FiksuClassic.Web.Internal
{
    public class NetClassicHttpRequest : IHttpRequest
    {
        private readonly HttpRequestBase _request;
        private IHttpHeaderDictionary _headers;
        private IHttpRequestCookies _cookies;
        private IHttpQueryString _query;

        public string ContentType => _request.ContentType;

        public long? ContentLength => _request.ContentLength;

        public string Method => _request.HttpMethod;

        public bool IsSecure => _request.IsSecureConnection;

        public string Path => _request.Path;

        public IHttpHeaderDictionary Headers
        {
            get => _headers ?? (_headers = new NetClassicHttpHeaderDictionary(_request.Headers));
        }

        public IHttpQueryString QueryString
        {
            get => _query ?? (_query = new NetClassicHttpQueryString(_request.QueryString));
        }

        public IHttpRequestCookies Cookies
        {
            get => _cookies ?? (_cookies = new NetClassicHttpRequestCookies(_request.Cookies));
        }

        public NetClassicHttpRequest(HttpRequestBase request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }
    }
}
