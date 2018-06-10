using Fiksu.Web;
using FiksuClassic.Web.Extensions;
using System;
using System.Web;

namespace FiksuClassic.Web.Internal
{
    public class FiksuClassicHttpResponseCookies : IHttpResponseCookies
    {
        private static readonly DateTimeOffset DefaultExpires = DateTimeOffset.UtcNow.AddYears(-10);

        private readonly HttpCookieCollection _cookies;

        public FiksuClassicHttpResponseCookies(HttpCookieCollection cookies)
        {
            _cookies = cookies ?? throw new ArgumentNullException(nameof(cookies));
        }

        public void Add(string key, string value)
        {
            _cookies.Add(new HttpCookie(key, value));
        }

        public void Add(string key, string value, IHttpCookieOptions options)
        {
            _cookies.Add(new HttpCookie(key, value)
            {
                Domain = options.Domain,
                Path = options.Path,
                Expires = options.Expires?.DateTime ?? DateTime.MinValue,
                HttpOnly = options.HttpOnly,
                Secure = options.Secure
            });
        }

        public void Expire(string key)
        {
            var existing = _cookies[key];
            Expire(key, existing.ToOptions());
        }

        public void Expire(string key, IHttpCookieOptions options)
        {
            options = options ?? new HttpCookieOptions();
            options.Expires = DefaultExpires;

            Add(key, null, options);
        }
    }
}
