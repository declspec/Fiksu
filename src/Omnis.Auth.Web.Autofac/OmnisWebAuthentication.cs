using Autofac;
using Omnis.Auth.Autofac;
using Omnis.Auth.Identity;
using Omnis.Web;

namespace Omnis.Auth.Web.Autofac {
    public static class OmnisWebAuthentication {
        public static AuthenticationBuilder AddOmnisAuthentication(this ContainerBuilder builder) {
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
