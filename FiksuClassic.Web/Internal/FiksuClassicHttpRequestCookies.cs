using Fiksu.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace FiksuClassic.Web.Internal
{
    public class FiksuClassicHttpRequestCookies : IHttpRequestCookies
    {
        private readonly HttpCookieCollection _cookies;

        public string this[string key] => _cookies[key]?.Value ?? string.Empty;
        public ICollection<string> Keys => _cookies.AllKeys;

        public FiksuClassicHttpRequestCookies(HttpCookieCollection cookies)
        {
            _cookies = cookies ?? throw new ArgumentNullException(nameof(cookies));
        }

        public bool ContainsKey(string key)
        {
            return _cookies[key] != null;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            var keys = _cookies.AllKeys;
            foreach (var key in keys)
            {
                var cookie = _cookies[key];
                if (cookie != null)
                    yield return new KeyValuePair<string, string>(key, cookie?.Value);
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            var cookie = _cookies[key];
            value = cookie?.Value;
            return cookie != null;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
