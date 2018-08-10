using System.Web;
using Autofac;
using Fiksu;
using Fiksu.Web;
using FiksuClassic.Web.Internal;

namespace FiksuClassic.Web.Autofac {
    public static class FiksuClassicServices {
        public static void AddFiksu(this ContainerBuilder builder, ExecutionEnvironment environment) {
            builder.Register(ctx => environment).As<ExecutionEnvironment>().SingleInstance();

            builder.Register(container => new FiksuClassicHttpContext(new HttpContextWrapper(HttpContext.Current)))
                .As<IHttpContext>().InstancePerRequest();
        }
    }
}
