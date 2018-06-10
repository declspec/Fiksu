using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace FiksuClassic.Web.Http.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static TService GetService<TService>(this IDependencyScope scope)
        {
            return (TService)scope.GetService(typeof(TService));
        }

        public static IEnumerable<TService> GetServices<TService>(this IDependencyScope scope)
        {
            foreach (var service in scope.GetServices(typeof(TService)))
                yield return (TService)service;
        }
    }
}
