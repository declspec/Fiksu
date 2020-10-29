using Omnis.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace OmnisCore.Web.Internal {
    public class OmnisCoreHttpResponse : IHttpResponse {
        private readonly HttpResponse _response;
        private IHttpHeaderDictionary _headers;
        private IHttpResponseCookies _cookies;

        public string ContentType {
            get => _response.ContentType;
            set => _response.ContentType = value;
        }

        public long? ContentLength {
            get => _response.ContentLength;
            set => _response.ContentLength = value;
        }

        public HttpStatusCode StatusCode {
            get => (HttpStatusCode)_response.StatusCode;
            set => _response.StatusCode = (int)value;
        }

        public OmnisCoreHttpResponse(HttpResponse response) {
            _response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public IHttpHeaderDictionary Headers {
            get => _headers ?? (_headers = new OmnisCoreHttpHeaderDictionary(_response.Headers));
        }

        public IHttpResponseCookies Cookies {
            get => _cookies ?? (_cookies = new OmnisCoreHttpResponseCookies(_response.Cookies));
        }

        public void Redirect(string url) {
            _response.Redirect(url);
        }

        public void Redirect(string url, bool permanent) {
            _response.Redirect(url, permanent);
        }
    }
}
