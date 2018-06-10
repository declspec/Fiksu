using Fiksu.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FiksuCore.Web.Internal
{
    public class FiksuCoreHttpQueryString : IHttpQueryString
    {
        private readonly IQueryCollection _query;

        public IList<string> this[string index]
        {
            get => _query[index];
        }
        
        public FiksuCoreHttpQueryString(IQueryCollection query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}
