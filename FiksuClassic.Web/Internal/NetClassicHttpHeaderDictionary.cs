using Fiksu.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FiksuClassic.Web.Internal
{
    public class NetClassicHttpHeaderDictionary : IHttpHeaderDictionary
    {
        private readonly NameValueCollection _headers;

        public NetClassicHttpHeaderDictionary(NameValueCollection headers)
        {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public IList<string> this[string index]
        {
            get => _headers.GetValues(index);
            set => Add(index, value);
        }

        public void Add(string key, IList<string> values)
        {
            foreach (var val in values)
                _headers.Add(key, val);
        }

        public void Add(string key, string value)
        {
            _headers.Add(key, value);
        }

        public IEnumerator<KeyValuePair<string, IList<string>>> GetEnumerator()
        {
            var keys = _headers.AllKeys;
            foreach (var key in keys)
            {
                if (key != null)
                    yield return new KeyValuePair<string, IList<string>>(key, _headers.GetValues(key));
            }
        }

        public void Remove(string key)
        {
            _headers.Remove(key);
        }

        public bool TryGetValues(string key, out IList<string> value)
        {
            value = null;

            if (_headers[key] == null)
                return Array.IndexOf(_headers.AllKeys, key) >= 0;
            else
            {
                value = this[key];
                return true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
