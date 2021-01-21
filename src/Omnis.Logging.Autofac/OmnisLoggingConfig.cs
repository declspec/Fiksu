using System;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Omnis.Logging.Autofac {
    public static class OmnisLoggingConfig {
        public static LoggingBuilder AddOmnisLogging(this ContainerBuilder builder) {
            builder.RegisterModule<LoggingModule>();
            return new LoggingBuilder(builder);
        }

        private class LoggingModule : Module {
            private static readonly Func<System.Reflection.ParameterInfo, IComponentContext, bool> ParameterPredicate = (info, ctx) => info.ParameterType == typeof(ILogger);

            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration) {
                registration.Preparing += ConfigureLoggerParameter;
            }

            private static void ConfigureLoggerParameter(object sender, PreparingEventArgs e) {
                var type = e.Component.Activator.LimitType;
                var param = new ResolvedParameter(ParameterPredicate, (i, c) => LoggerFactory.GetLogger(type.FullName));

                e.Parameters = e.Parameters.Concat(new[] { param });
            }
        }
    }
}
