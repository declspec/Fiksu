using Autofac;
using NLog.Config;

namespace Fiksu.Logging.Autofac {
    public class LoggingBuilder {
        public ContainerBuilder ContainerBuilder { get; }
        public LoggingConfiguration Configuration { get; }

        public LoggingBuilder(ContainerBuilder containerBuilder, LoggingConfiguration configuration) {
            ContainerBuilder = containerBuilder;
            Configuration = configuration;
        }

        public void Build() {
            System.Diagnostics.Trace.Listeners.Add(new NLog.NLogTraceListener());
            NLog.LogManager.Configuration = Configuration;
        }
    }
}
