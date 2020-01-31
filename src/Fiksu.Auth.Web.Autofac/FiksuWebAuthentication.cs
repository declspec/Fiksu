using System.Collections.Generic;
using Autofac;
using Fiksu.Auth.Autofac;
using Fiksu.Auth.Identity;
using Fiksu.Web;

namespace Fiksu.Auth.Web.Autofac {
    public static class FiksuWebAuthentication {
        public static AuthenticationBuilder AddFiksuAuthentication(this ContainerBuilder builder, IList<string> masqueradeRoles = null, IList<string> restrictedMasqueradeRoles = null) {
            // TODO: Investigate if 'SingleInstance' causes issues in future
            builder.RegisterType<IdentityService>().As<IIdentityService>()
                .SingleInstance();

            builder.RegisterType<IdentitySessionEventManager<IHttpContext>>()
                .As<IIdentitySessionEventManager<IHttpContext>>()
                .SingleInstance();

            builder.RegisterType<ActiveAuthenticationService>()
                .As<IActiveAuthenticationService>()
                .WithParameter(new NamedParameter("masqueradeRoles", masqueradeRoles))
                .WithParameter(new NamedParameter("restrictedMasqueradeRoles", restrictedMasqueradeRoles))
                .SingleInstance();

            return new AuthenticationBuilder(builder);
        }
    }
}
