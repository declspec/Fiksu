using System;

namespace Omnis.Web {
    public class HttpCookieOptions : IHttpCookieOptions {
        public string Domain { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public bool HttpOnly { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
    }
}
