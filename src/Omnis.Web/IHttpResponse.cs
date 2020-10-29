using System.Net;

namespace Omnis.Web {
    public interface IHttpResponse {
        string ContentType { get; set; }
        HttpStatusCode StatusCode { get; set; }
        IHttpHeaderDictionary Headers { get; }
        IHttpResponseCookies Cookies { get; }

        void Redirect(string url);
        void Redirect(string url, bool permanent);
    }
}
