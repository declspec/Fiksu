using Omnis.Web;
using Microsoft.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OmnisClassic.Web.Internal {
    public class OmnisOwinHttpHeaderDictionary : IHttpHeaderDictionary {
        private readonly IHeaderDictionary _headers;

        public OmnisOwinHttpHeaderDictionary(IHeaderDictionary headers) {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public IList<string> this[string index] {
            get => _headers.GetValues(index);
            set => Add(index, value);
        }

        public void Add(string key, string value) {
            _headers.Append(key, value);
        }

        public void Add(string key, IList<string> values) {
            foreach (var value in values)
                _headers.Append(key, value);
        }

        public IEnumerator<KeyValuePair<string, IList<string>>> GetEnumerator() {
            foreach (var kvp in _headers)
                yield return new KeyValuePair<string, IList<string>>(kvp.Key, kvp.Value);
        }

        public void Remove(string key) {
            _headers.Remove(key);
        }

        public bool TryGetValues(string key, out IList<string> value) {
            var success = _headers.TryGetValue(key, out var array);
            value = array;
            return success;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
