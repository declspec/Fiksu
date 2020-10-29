using Omnis.Web;
using Microsoft.Owin;
using System;

namespace OmnisClassic.Web.Internal {
    public class OmnisOwinHttpResponseCookies : IHttpResponseCookies {
        private static readonly DateTimeOffset DefaultExpires = DateTimeOffset.UtcNow.AddYears(-10);

        private readonly ResponseCookieCollection _cookies;

        public OmnisOwinHttpResponseCookies(ResponseCookieCollection cookies) {
            _cookies = cookies ?? throw new ArgumentNullException(nameof(cookies));
        }

        public void Add(string key, string value) {
            _cookies.Append(key, value);
        }

        public void Add(string key, string value, IHttpCookieOptions options) {
            _cookies.Append(key, value, new CookieOptions() {
                Domain = options.Domain,
                Path = options.Path,
                Expires = options.Expires?.DateTime ?? DateTime.MinValue,
                HttpOnly = options.HttpOnly,
                Secure = options.Secure
            });
        }

        public void Expire(string key) {
            Expire(key, null);
        }

        public void Expire(string key, IHttpCookieOptions options) {
            options = options ?? new HttpCookieOptions();
            options.Expires = DefaultExpires;

            Add(key, null, options);
        }
    }
}
