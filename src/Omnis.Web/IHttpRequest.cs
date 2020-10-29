namespace Omnis.Web {
    public interface IHttpRequest {
        string ContentType { get; }
        long? ContentLength { get; }
        string Method { get; }
        bool IsSecure { get; }
        string Path { get; }
        IHttpHeaderDictionary Headers { get; }
        IHttpQueryString QueryString { get; }
        IHttpRequestCookies Cookies { get; }
    }
}
