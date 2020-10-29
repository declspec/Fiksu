using System.Collections.Generic;

namespace Omnis.Web {
    public interface IHttpHeaderDictionary : IEnumerable<KeyValuePair<string, IList<string>>> {
        IList<string> this[string index] { get; set; }

        void Add(string key, string value);
        void Add(string key, IList<string> values);
        void Remove(string key);
        bool TryGetValues(string key, out IList<string> value);
    }
}
