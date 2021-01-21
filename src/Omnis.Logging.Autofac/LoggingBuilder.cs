using Autofac;

namespace Omnis.Logging.Autofac {
    public class LoggingBuilder {
        public ContainerBuilder ContainerBuilder { get; }

        public LoggingBuilder(ContainerBuilder containerBuilder) {
            ContainerBuilder = containerBuilder;
        }

        public void Build() {

        }
    }
}
