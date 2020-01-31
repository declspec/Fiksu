using Fiksu.Web;
using System.Web;

namespace FiksuClassic.Web.Extensions {
    public static class HttpCookieExtensions {
        public static IHttpCookieOptions ToOptions(this HttpCookie cookie) {
            return new HttpCookieOptions() {
                Domain = cookie.Domain,
                Path = cookie.Path,
                Expires = cookie.Expires,
                Secure = cookie.Secure,
                HttpOnly = cookie.HttpOnly
            };
        }
    }
}
