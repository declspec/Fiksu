using Fiksu.Web;
using Microsoft.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FiksuClassic.Web.Internal
{
    public class FiksuOwinHttpRequestCookies : IHttpRequestCookies
    {
        private readonly RequestCookieCollection _cookies;

        public string this[string key] => _cookies[key];
        public ICollection<string> Keys => _cookies.Select(kvp => kvp.Key).ToList();

        public FiksuOwinHttpRequestCookies(RequestCookieCollection cookies)
        {
            _cookies = cookies ?? throw new ArgumentNullException(nameof(cookies));
        }

        public bool ContainsKey(string key)
        {
            return _cookies[key] != null;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _cookies.GetEnumerator();
        }

        public bool TryGetValue(string key, out string value)
        {
            value = _cookies[key];
            return value != null;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
