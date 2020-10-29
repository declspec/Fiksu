using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OmnisCore.Web.Routing {
    public class RegexRouter : IRouter {
        private readonly IDictionary<string, List<RegexRoute>> _routeTable;
        private readonly IRouter _defaultRouter;

        public RegexRouter(IRouter defaultRouter, IList<RegexRoute> routes) {
            _defaultRouter = defaultRouter;
            _routeTable = routes.GroupBy(r => r.Method.ToUpper())
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public Task RouteAsync(RouteContext context) {
            if (_routeTable.TryGetValue(context.HttpContext.Request.Method.ToUpper(), out var routes)) {
                foreach (var route in routes) {
                    var match = route.RegularExpression.Match(context.HttpContext.Request.Path);

                    if (match.Success) {
                        foreach (Group group in match.Groups) {
                            if (!char.IsDigit(group.Name[0]))
                                context.RouteData.Values[group.Name] = group.Value;
                        }

                        context.RouteData.Values["controller"] = route.Controller;
                        context.RouteData.Values["action"] = route.Action;

                        break;
                    }
                }
            }

            return _defaultRouter.RouteAsync(context);
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context) {
            throw new NotSupportedException("RegexRouter does not support virtual paths");
        }
    }
}
