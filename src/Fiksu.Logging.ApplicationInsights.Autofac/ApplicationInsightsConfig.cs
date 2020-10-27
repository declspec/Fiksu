using Fiksu.Extensions;
using Fiksu.Logging.Autofac;
using Microsoft.ApplicationInsights.Extensibility;

namespace Fiksu.Logging.ApplicationInsights.Autofac {
    public static class ApplicationInsightsConfig {
        public static LoggingBuilder AddApplicationInsights(this LoggingBuilder builder, IExecutionEnvironment environment, string instrumentationKey) {
            var config = TelemetryConfiguration.Active;

            config.TelemetryChannel.DeveloperMode = environment.IsDevelopment();
            config.InstrumentationKey = instrumentationKey;
            config.DisableTelemetry = string.IsNullOrWhiteSpace(instrumentationKey);

            // Add to NLog
            var aiTarget = new ApplicationInsightsTarget() {
                Name = "ApplicationInsights",
                InstrumentationKey = config.InstrumentationKey
            };

            builder.Configuration.AddTarget(aiTarget.Name, aiTarget);
            builder.Configuration.AddRuleForAllLevels(aiTarget);

            return builder;
        }
    }
}
