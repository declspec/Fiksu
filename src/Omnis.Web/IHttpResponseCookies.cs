namespace Omnis.Web {
    public interface IHttpResponseCookies {
        void Add(string key, string value);
        void Add(string key, string value, IHttpCookieOptions options);
        void Expire(string key);
        void Expire(string key, IHttpCookieOptions options);
    }
}
