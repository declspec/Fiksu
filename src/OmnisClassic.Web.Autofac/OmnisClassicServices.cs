using System.Web;
using Autofac;
using Omnis;
using Omnis.Web;
using OmnisClassic.Web.Internal;

namespace OmnisClassic.Web.Autofac {
    public static class OmnisClassicServices {
        public static void AddOmnis(this ContainerBuilder builder, IExecutionEnvironment environment) {
            builder.Register(ctx => environment).As<IExecutionEnvironment>().SingleInstance();

            builder.Register(container => new OmnisClassicHttpContext(new HttpContextWrapper(HttpContext.Current)))
                .As<IHttpContext>().InstancePerRequest();
        }
    }
}
