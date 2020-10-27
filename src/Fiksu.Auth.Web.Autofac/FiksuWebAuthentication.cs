using Autofac;
using Fiksu.Auth.Autofac;
using Fiksu.Auth.Identity;
using Fiksu.Web;

namespace Fiksu.Auth.Web.Autofac {
    public static class FiksuWebAuthentication {
        public static AuthenticationBuilder AddFiksuAuthentication(this ContainerBuilder builder) {
            // TODO: Investigate if 'SingleInstance' causes issues in future
            builder.RegisterType<IdentityFactory>().As<IIdentityFactory>()
                .SingleInstance();

            builder.RegisterType<SessionEventManager<IHttpContext>>()
                .As<ISessionEventManager<IHttpContext>>()
                .SingleInstance();

            builder.RegisterType<BasicAuthenticationService>()
                .As<IBasicAuthenticationService>()
                .SingleInstance();

            return new AuthenticationBuilder(builder);
        }
    }
}
