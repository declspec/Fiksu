using System.Collections.Generic;

namespace Fiksu.Web
{
    public interface IHttpRequestCookies : IEnumerable<KeyValuePair<string, string>>
    {
        string this[string key] { get; }
        ICollection<string> Keys { get; }

        bool ContainsKey(string key);
        bool TryGetValue(string key, out string value);
    }
}
