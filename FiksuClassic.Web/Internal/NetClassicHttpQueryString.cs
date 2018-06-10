using Fiksu.Web;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Specialized;

namespace FiksuClassic.Web.Internal
{
    public class NetClassicHttpQueryString : IHttpQueryString
    {
        private readonly NameValueCollection _query;

        public StringValues this[string index] => _query.GetValues(index);

        public NetClassicHttpQueryString(NameValueCollection query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
