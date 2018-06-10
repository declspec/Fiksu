using Fiksu.Web;
using Microsoft.Extensions.Primitives;
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

        // TODO: StringValues isn't a first-class concept in Net Classic so see how well this holds up.
        public StringValues this[string index]
        {
            get => _headers.GetValues(index);
            set => Add(index, value);
        }

        public void Add(string key, StringValues value) => _headers.Set(key, value);

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
        {
            var keys = _headers.AllKeys;
            foreach (var key in keys)
                yield return new KeyValuePair<string, StringValues>(key, _headers[key]);
        }

        public void Remove(string key)
        {
            _headers.Remove(key);
        }

        public bool TryGetValue(string key, out StringValues value)
        {
            value = StringValues.Empty;

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
