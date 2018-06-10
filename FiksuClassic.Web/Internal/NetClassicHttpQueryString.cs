using Fiksu.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FiksuClassic.Web.Internal
{
    public class NetClassicHttpQueryString : IHttpQueryString
    {
        private readonly NameValueCollection _query;

        public IList<string> this[string index] => _query.GetValues(index);

        public NetClassicHttpQueryString(NameValueCollection query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
