using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

/// <summary>
/// This is taken almost verbatim from https://github.com/Microsoft/ApplicationInsights-dotnet-logging
/// However .NET Standard support is still in beta for that project (coming in v2.6). Once 2.6 is out of beta
/// It would be ideal to delete this class and reference the proper NLogTarget project (JL 08/05/2018)
/// </summary>
namespace Fiksu.Logging.ApplicationInsights.Autofac {
    /// <summary>
    /// NLog Target that routes all logging output to the Application Insights logging framework.
    /// The messages will be uploaded to the Application Insights cloud service.
    /// </summary>
    [Target("ApplicationInsightsTarget")]
    public sealed class ApplicationInsightsTarget : TargetWithLayout {
        private TelemetryClient _telemetryClient;
        private DateTime _lastLogEventTime;

        /// <summary>
        /// Gets or sets the Application Insights instrumentationKey for your application. 
        /// </summary>
        public string InstrumentationKey { get; set; }

        /// <summary>
        /// Gets the array of custom attributes to be passed into the logevent context
        /// </summary>
        [ArrayParameter(typeof(TargetPropertyWithContext), "contextproperty")]
        public IList<TargetPropertyWithContext> ContextProperties { get; } = new List<TargetPropertyWithContext>();

        /// <summary>
        /// Initializers a new instance of ApplicationInsightsTarget type.
        /// </summary>
        public ApplicationInsightsTarget() {
            Layout = "${message}";
        }

        private void PopulateTelemetryProperties(ITelemetry telemetry, LogEventInfo logEvent) {
            telemetry.Timestamp = logEvent.TimeStamp;
            telemetry.Sequence = logEvent.SequenceID.ToString(CultureInfo.InvariantCulture);

            var propertyBag = telemetry is ExceptionTelemetry ex
                ? ex.Properties
                : ((TraceTelemetry)telemetry).Properties;

            if (!string.IsNullOrEmpty(logEvent.LoggerName))
                propertyBag.Add(nameof(logEvent.LoggerName), logEvent.LoggerName);

            if (logEvent.UserStackFrame != null) {
                propertyBag.Add(nameof(logEvent.UserStackFrame), logEvent.UserStackFrame.ToString());
                propertyBag.Add(nameof(logEvent.UserStackFrameNumber), logEvent.UserStackFrameNumber.ToString(CultureInfo.InvariantCulture));
            }

            foreach (var property in ContextProperties) {
                if (!string.IsNullOrEmpty(property.Name)) {
                    var propertyValue = property.Layout?.Render(logEvent);
                    AddToPropertyBag(propertyBag, property.Name, propertyValue);
                }
            }

            if (logEvent.HasProperties && logEvent.Properties?.Count > 0) {
                foreach (var kvp in logEvent.Properties) {
                    AddToPropertyBag(propertyBag, kvp.Key.ToString(), kvp.Value);
                }
            }
        }

        /// <summary>
        /// Initializes the Target and perform instrumentationKey validation.
        /// </summary>
        /// <exception cref="NLogConfigurationException">Will throw when <see cref="InstrumentationKey"/> is not set.</exception>
        protected override void InitializeTarget() {
            base.InitializeTarget();
            _telemetryClient = new TelemetryClient();

            if (!string.IsNullOrEmpty(InstrumentationKey))
                _telemetryClient.Context.InstrumentationKey = InstrumentationKey;

            _telemetryClient.Context.GetInternalContext().SdkVersion = GetSdkVersion("nlog:");
        }

        /// <summary>
        /// Send the log message to Application Insights.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logEvent"/> is null.</exception>
        protected override void Write(LogEventInfo logEvent) {
            if (logEvent == null)
                throw new ArgumentNullException(nameof(logEvent));

            _lastLogEventTime = DateTime.UtcNow;

            if (logEvent.Exception != null)
                SendException(logEvent);
            else
                SendTrace(logEvent);
        }

        /// <summary>
        /// Flush any pending log messages
        /// </summary>
        /// <param name="asyncContinuation">The asynchronous continuation</param>
        protected override void FlushAsync(AsyncContinuation asyncContinuation) {
            try {
                _telemetryClient.Flush();

                if (DateTime.UtcNow.AddSeconds(-30) > _lastLogEventTime) {
                    // Nothing has been written, so nothing to wait for
                    asyncContinuation(null);
                }
                else {
                    // Documentation says it is important to wait after flush, else nothing will happen
                    // https://docs.microsoft.com/azure/application-insights/app-insights-api-custom-events-metrics#flushing-data
                    Task.Delay(TimeSpan.FromMilliseconds(500)).ContinueWith(task => asyncContinuation(null));
                }
            }
            catch (Exception ex) {
                asyncContinuation(ex);
            }
        }

        private void SendException(LogEventInfo logEvent) {
            var exceptionTelemetry = new ExceptionTelemetry(logEvent.Exception) {
                SeverityLevel = GetSeverityLevel(logEvent.Level)
            };

            exceptionTelemetry.Properties.Add("Message", Layout.Render(logEvent));
            Send(logEvent, exceptionTelemetry);
        }

        private void SendTrace(LogEventInfo logEvent) {
            Send(logEvent, new TraceTelemetry(Layout.Render(logEvent)) {
                SeverityLevel = GetSeverityLevel(logEvent.Level)
            });
        }

        private void Send(LogEventInfo logEvent, ITelemetry telemetry) {
            PopulateTelemetryProperties(telemetry, logEvent);
            _telemetryClient.Track(telemetry);
        }

        private static void AddToPropertyBag(IDictionary<string, string> propertyBag, string key, object valueObj) {
            if (valueObj != null) {
                var nextKey = key;
                var qualifier = 0;
                var comparison = Convert.ToString(valueObj, CultureInfo.InvariantCulture);

                while (propertyBag.TryGetValue(nextKey, out var value)) {
                    // Check if an entry with the same key/value already exists.
                    if (string.Equals(value, comparison, StringComparison.Ordinal))
                        return;

                    nextKey = string.Format("{0}_{1}", key, ++qualifier);
                }

                propertyBag.Add(nextKey, comparison);
            }
        }

        private static SeverityLevel? GetSeverityLevel(LogLevel logEventLevel) {
            if (logEventLevel == null)
                return null;

            if (logEventLevel.Ordinal == LogLevel.Trace.Ordinal || logEventLevel.Ordinal == LogLevel.Debug.Ordinal)
                return SeverityLevel.Verbose;

            if (logEventLevel.Ordinal == LogLevel.Info.Ordinal)
                return SeverityLevel.Information;

            if (logEventLevel.Ordinal == LogLevel.Warn.Ordinal)
                return SeverityLevel.Warning;

            if (logEventLevel.Ordinal == LogLevel.Error.Ordinal)
                return SeverityLevel.Error;

            if (logEventLevel.Ordinal == LogLevel.Fatal.Ordinal)
                return SeverityLevel.Critical;

            // The only possible value left if OFF but we should never get here in this case
            return null;
        }

        private static string GetSdkVersion(string versionPrefix) {
            var versionStr = typeof(TelemetryClient).Assembly.GetCustomAttributes<AssemblyFileVersionAttribute>().First().Version;
            var version = new Version(versionStr);

            return string.Format("{0}{1}-{2}", versionPrefix, version.ToString(3), version.Revision);
        }
    }
}
