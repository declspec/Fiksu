using Fiksu.Web;
using Microsoft.AspNetCore.Http;
using System;

namespace FiksuCore.Web.Internal {
    public class FiksuCoreHttpResponseCookies : IHttpResponseCookies {
        private readonly IResponseCookies _cookies;

        public FiksuCoreHttpResponseCookies(IResponseCookies cookies) {
            _cookies = cookies ?? throw new ArgumentNullException(nameof(cookies));
        }

        public void Add(string key, string value) {
            _cookies.Append(key, value);
        }

        public void Add(string key, string value, IHttpCookieOptions options) {
            _cookies.Append(key, value, ToCookieOptions(options));
        }

        public void Expire(string key) {
            _cookies.Delete(key);
        }

        public void Expire(string key, IHttpCookieOptions options) {
            _cookies.Delete(key, ToCookieOptions(options));
        }

        /*
        public void Add(string key, string value) => _cookies.Append(key, value);
        public void Add(string key, string value, IHttpCookieOptions options) => _cookies.Append(key, value, ToCookieOptions(options));
        public void Remove(string key) => _cookies.Delete(key);
        public void Remove(string key, IHttpCookieOptions options) => _cookies.Delete(key, ToCookieOptions(options));
        */
        private static CookieOptions ToCookieOptions(IHttpCookieOptions options) {
            return options as CookieOptions ?? new CookieOptions() {
                Domain = options.Domain,
                Expires = options.Expires,
                HttpOnly = options.HttpOnly,
                Path = options.Path,
                Secure = options.Secure
            };
        }
    }
}
