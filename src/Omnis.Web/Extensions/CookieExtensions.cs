using System.Net;

namespace Omnis.Web.Extensions {
    public static class CookieExtensions {
        public static IHttpCookieOptions ToCookieOptions(this Cookie cookie) {
            return new HttpCookieOptions() {
                Path = cookie.Path,
                Domain = cookie.Domain,
                Expires = cookie.Expires,
                HttpOnly = cookie.HttpOnly,
                Secure = cookie.Secure
            };
        }
    }
}
