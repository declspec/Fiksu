using Fiksu.Logging.Autofac;
using Microsoft.ApplicationInsights.Extensibility;

namespace Fiksu.Logging.ApplicationInsights.Autofac
{
    public static class ApplicationInsightsConfig
    {
        public static LoggingBuilder AddApplicationInsights(this LoggingBuilder builder, ExecutionEnvironment environment, string instrumentationKey)
        {
            var config = TelemetryConfiguration.Active;

            config.TelemetryChannel.DeveloperMode = environment == ExecutionEnvironment.Development;
            config.InstrumentationKey = instrumentationKey;
            config.DisableTelemetry = string.IsNullOrWhiteSpace(instrumentationKey);

            // Add to NLog
            var aiTarget = new ApplicationInsightsTarget()
            {
                Name = "ApplicationInsights",
                InstrumentationKey = config.InstrumentationKey
            };

            builder.Configuration.AddTarget(aiTarget.Name, aiTarget);
            builder.Configuration.AddRuleForAllLevels(aiTarget);

            return builder;
        }
    }
}
