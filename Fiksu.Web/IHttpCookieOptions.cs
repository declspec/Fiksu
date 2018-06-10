using System;

namespace Fiksu.Web
{
    public interface IHttpCookieOptions
    {
        string Domain { get; set; }
        DateTimeOffset? Expires { get; set; }
        bool HttpOnly { get; set; }
        string Path { get; set; }
        bool Secure { get; set; }
    }
}
