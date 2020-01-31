using System;
using System.Text.RegularExpressions;

namespace FiksuCore.Web.Routing {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RegexRouteAttribute : Attribute {
        public string Pattern { get; }
        public RegexOptions Options { get; }

        public RegexRouteAttribute(string pattern)
            : this(pattern, RegexOptions.None) { }

        public RegexRouteAttribute(string pattern, RegexOptions options) {
            Pattern = pattern;
            Options = options;
        }
    }
}
