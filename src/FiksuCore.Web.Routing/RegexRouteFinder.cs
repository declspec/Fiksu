using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FiksuCore.Web.Routing {
    public static class RegexRouteFinder {
        private static readonly Regex ControllerNamePattern = new Regex("Controller$", RegexOptions.Compiled);

        public static IEnumerable<RegexRoute> FindAll(params Assembly[] assemblies) {
            var controllerBaseType = typeof(ControllerBase);
            var controllerTypes = assemblies.SelectMany(a => a.GetTypes().Where(controllerBaseType.IsAssignableFrom));

            foreach (var ct in controllerTypes) {
                var ctrlName = ControllerNamePattern.Replace(ct.Name, "");
                var rootAttr = ct.GetCustomAttribute<RegexRouteAttribute>();
                var rootOpts = RegexOptions.Compiled | (rootAttr?.Options ?? RegexOptions.None);

                foreach (var mt in ct.GetMethods()) {
                    var childAttr = mt.GetCustomAttribute<RegexRouteAttribute>();

                    if (childAttr != null) {
                        var childPattern = $"^/{string.Join("/", new[] { rootAttr?.Pattern, childAttr.Pattern }.Where(p => !string.IsNullOrEmpty(p)))}$";
                        yield return new RegexRoute(new Regex(childPattern, rootOpts | childAttr.Options), GetMethodVerb(mt), ctrlName, mt.Name);
                    }
                }
            }
        }

        private static string GetMethodVerb(MethodInfo minfo) {
            if (minfo.GetCustomAttribute<HttpDeleteAttribute>() != null)
                return "DELETE";
            if (minfo.GetCustomAttribute<HttpPutAttribute>() != null)
                return "PUT";
            if (minfo.GetCustomAttribute<HttpPatchAttribute>() != null)
                return "PATCH";
            if (minfo.GetCustomAttribute<HttpPostAttribute>() != null)
                return "POST";
            return "GET";
        }
    }
}
