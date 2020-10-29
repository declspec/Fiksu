using Omnis.Web;
using Microsoft.Owin;
using System;
using System.Net;

namespace OmnisClassic.Web.Internal {
    public class OmnisOwinHttpResponse : IHttpResponse {
        private readonly IOwinResponse _response;
        private IHttpHeaderDictionary _headers;
        private IHttpResponseCookies _cookies;

        public string ContentType {
            get => _response.ContentType;
            set => _response.ContentType = value;
        }

        public HttpStatusCode StatusCode {
            get => (HttpStatusCode)_response.StatusCode;
            set => _response.StatusCode = (int)value;
        }

        public IHttpHeaderDictionary Headers {
            get => _headers ?? (_headers = new OmnisOwinHttpHeaderDictionary(_response.Headers));
        }

        public IHttpResponseCookies Cookies {
            get => _cookies ?? (_cookies = new OmnisOwinHttpResponseCookies(_response.Cookies));
        }

        public OmnisOwinHttpResponse(IOwinResponse response) {
            _response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public void Redirect(string url) {
            Redirect(url, false);
        }

        public void Redirect(string url, bool permanent) {
            StatusCode = permanent ? HttpStatusCode.MovedPermanently : HttpStatusCode.Moved;
            Headers.Add("Location", url);
        }
    }
}
