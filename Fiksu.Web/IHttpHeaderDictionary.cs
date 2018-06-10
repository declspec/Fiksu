using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Fiksu.Web
{
    public interface IHttpHeaderDictionary : IEnumerable<KeyValuePair<string, StringValues>>
    {
        StringValues this[string index] { get; set; }

        void Add(string key, StringValues value);
        void Remove(string key);
        bool TryGetValue(string key, out StringValues value);
    }
}
