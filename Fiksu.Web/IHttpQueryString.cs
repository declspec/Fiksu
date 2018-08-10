using System.Collections.Generic;

namespace Fiksu.Web {
    public interface IHttpQueryString {
        IList<string> this[string index] { get; }
    }
}
