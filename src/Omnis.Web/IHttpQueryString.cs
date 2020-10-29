using System.Collections.Generic;

namespace Omnis.Web {
    public interface IHttpQueryString {
        IList<string> this[string index] { get; }
    }
}
