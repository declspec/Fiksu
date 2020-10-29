using Omnis.Web;
using Microsoft.Owin;
using System;
using System.Collections.Generic;

namespace OmnisClassic.Web.Internal {
    public class OmnisOwinHttpQueryString : IHttpQueryString {
        private readonly IReadableStringCollection _query;

        public IList<string> this[string index] => _query.GetValues(index);

        public OmnisOwinHttpQueryString(IReadableStringCollection query) {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
