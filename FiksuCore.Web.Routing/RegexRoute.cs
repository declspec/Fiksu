using System.Text.RegularExpressions;

namespace FiksuCore.Web.Routing {
    public class RegexRoute {
        public Regex RegularExpression { get; }
        public string Method { get; }
        public string Controller { get; }
        public string Action { get; }

        public RegexRoute(Regex regex, string method, string controller, string action) {
            RegularExpression = regex;
            Method = method;
            Controller = controller;
            Action = action;
        }
    }
}
