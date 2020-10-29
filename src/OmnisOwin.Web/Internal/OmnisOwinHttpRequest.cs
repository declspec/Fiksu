using Omnis.Web;
using Microsoft.Owin;
using System;

namespace OmnisClassic.Web.Internal {
    public class OmnisOwinHttpRequest : IHttpRequest {
        private readonly IOwinRequest _request;

        private IHttpHeaderDictionary _headers;
        private IHttpRequestCookies _cookies;
        private IHttpQueryString _query;

        public string ContentType => _request.ContentType;

        public long? ContentLength => _request.Headers.TryGetValue("Content-Length", out var headers) && long.TryParse(headers[0], out var length) ? (long?)length : null;

        public string Method => _request.Method;

        public bool IsSecure => _request.IsSecure;

        public string Path => (_request.PathBase + _request.Path).ToString();

        public IHttpHeaderDictionary Headers {
            get => _headers ?? (_headers = new OmnisOwinHttpHeaderDictionary(_request.Headers));
        }

        public IHttpQueryString QueryString {
            get => _query ?? (_query = new OmnisOwinHttpQueryString(_request.Query));
        }

        public IHttpRequestCookies Cookies {
            get => _cookies ?? (_cookies = new OmnisOwinHttpRequestCookies(_request.Cookies));
        }

        public OmnisOwinHttpRequest(IOwinRequest request) {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }
    }
}
