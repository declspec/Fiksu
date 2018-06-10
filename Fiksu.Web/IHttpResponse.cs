using System.Net;

namespace Fiksu.Web
{
    public interface IHttpResponse
    {
        string ContentType { get; set; }
        HttpStatusCode StatusCode { get; set; }
        string StatusDescription { get; set; }
        IHttpHeaderDictionary Headers { get; }
        IHttpResponseCookies Cookies { get; }

        void Redirect(string url);
        void Redirect(string url, bool permanent);
    }
}
