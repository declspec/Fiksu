using Fiksu.Web;
using Microsoft.Owin;
using System;
using System.Collections.Generic;

namespace FiksuClassic.Web.Internal {
    public class FiksuOwinHttpQueryString : IHttpQueryString {
        private readonly IReadableStringCollection _query;

        public IList<string> this[string index] => _query.GetValues(index);

        public FiksuOwinHttpQueryString(IReadableStringCollection query) {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
