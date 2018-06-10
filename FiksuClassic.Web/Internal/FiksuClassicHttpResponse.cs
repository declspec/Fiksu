using Fiksu.Web;
using System;
using System.Net;
using System.Web;

namespace FiksuClassic.Web.Internal
{
    public class FiksuClassicHttpResponse : IHttpResponse
    {
        private readonly HttpResponseBase _response;
        private IHttpHeaderDictionary _headers;
        private IHttpResponseCookies _cookies;

        public string ContentType
        {
            get => _response.ContentType;
            set => _response.ContentType = value;
        }

        public HttpStatusCode StatusCode
        {
            get => (HttpStatusCode)_response.StatusCode;
            set => _response.StatusCode = (int)value;
        }

        public string StatusDescription
        {
            get => _response.StatusDescription;
            set => _response.StatusDescription = value;
        }

        public IHttpHeaderDictionary Headers
        {
            get => _headers ?? (_headers = new FiksuClassicHttpHeaderDictionary(_response.Headers));
        }

        public IHttpResponseCookies Cookies
        {
            get => _cookies ?? (_cookies = new FiksuClassicHttpResponseCookies(_response.Cookies));
        }

        public FiksuClassicHttpResponse(HttpResponseBase response)
        {
            _response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public void Redirect(string url) => Redirect(url, false);

        public void Redirect(string url, bool permanent)
        {
            if (permanent)
                _response.RedirectPermanent(url);
            else
                _response.Redirect(url);
        }
    }
}
