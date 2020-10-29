using Omnis.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace OmnisClassic.Web.Internal {
    public class OmnisClassicHttpQueryString : IHttpQueryString {
        private readonly NameValueCollection _query;

        public IList<string> this[string index] => _query.GetValues(index);

        public OmnisClassicHttpQueryString(NameValueCollection query) {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
