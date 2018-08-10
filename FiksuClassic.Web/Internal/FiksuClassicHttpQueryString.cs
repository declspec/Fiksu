using Fiksu.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FiksuClassic.Web.Internal {
    public class FiksuClassicHttpQueryString : IHttpQueryString {
        private readonly NameValueCollection _query;

        public IList<string> this[string index] => _query.GetValues(index);

        public FiksuClassicHttpQueryString(NameValueCollection query) {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
