using Omnis.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace OmnisCore.Web.Internal {
    public class OmnisCoreHttpQueryString : IHttpQueryString {
        private readonly IQueryCollection _query;

        public IList<string> this[string index] {
            get => _query[index];
        }

        public OmnisCoreHttpQueryString(IQueryCollection query) {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
