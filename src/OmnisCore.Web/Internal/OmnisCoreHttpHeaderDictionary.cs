using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Omnis.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace OmnisCore.Web.Internal {
    public class OmnisCoreHttpHeaderDictionary : IHttpHeaderDictionary {
        private readonly IHeaderDictionary _headers;

        public IList<string> this[string index] {
            get => _headers[index];
            set => _headers[index] = new StringValues(value as string[] ?? value.ToArray());
        }

        public OmnisCoreHttpHeaderDictionary(IHeaderDictionary headers) {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public void Add(string key, string value) {
            _headers.Append(key, value);
        }

        public void Add(string key, IList<string> values) {
            _headers.Append(key, new StringValues(values as string[] ?? values.ToArray()));
        }

        public void Remove(string key) {
            _headers.Remove(key);
        }

        public bool TryGetValues(string key, out IList<string> value) {
            var success = _headers.TryGetValue(key, out var stringValues);
            value = stringValues;
            return success;
        }

        public IEnumerator<KeyValuePair<string, IList<string>>> GetEnumerator() {
            foreach (var kvp in _headers)
                yield return new KeyValuePair<string, IList<string>>(kvp.Key, kvp.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
