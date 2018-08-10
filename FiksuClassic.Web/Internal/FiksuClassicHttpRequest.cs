using Fiksu.Web;
using System;
using System.Web;

namespace FiksuClassic.Web.Internal {
    public class FiksuClassicHttpRequest : IHttpRequest {
        private readonly HttpRequestBase _request;
        private IHttpHeaderDictionary _headers;
        private IHttpRequestCookies _cookies;
        private IHttpQueryString _query;

        public string ContentType => _request.ContentType;

        public long? ContentLength => _request.ContentLength;

        public string Method => _request.HttpMethod;

        public bool IsSecure => _request.IsSecureConnection;

        public string Path => _request.Path;

        public IHttpHeaderDictionary Headers {
            get => _headers ?? (_headers = new FiksuClassicHttpHeaderDictionary(_request.Headers));
        }

        public IHttpQueryString QueryString {
            get => _query ?? (_query = new FiksuClassicHttpQueryString(_request.QueryString));
        }

        public IHttpRequestCookies Cookies {
            get => _cookies ?? (_cookies = new FiksuClassicHttpRequestCookies(_request.Cookies));
        }

        public FiksuClassicHttpRequest(HttpRequestBase request) {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }
    }
}
