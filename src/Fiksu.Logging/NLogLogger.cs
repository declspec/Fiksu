using System;
using System.Threading.Tasks;

namespace Fiksu.Logging {
    public class NLogLogger : ILogger {
        private readonly NLog.ILogger _logger;

        public NLogLogger(NLog.ILogger logger) {
            _logger = logger;
        }

        public Task Debug(string message, params object[] arguments) {
            _logger.Debug(message, arguments);
            return Task.CompletedTask;
        }

        public Task Debug(Exception ex, string message, params object[] arguments) {
            _logger.Debug(ex, message, arguments);
            return Task.CompletedTask;
        }

        public Task Error(string message, params object[] arguments) {
            _logger.Error(message, arguments);
            return Task.CompletedTask;
        }

        public Task Error(Exception ex, string message, params object[] arguments) {
            _logger.Error(ex, message, arguments);
            return Task.CompletedTask;
        }

        public Task Fatal(string message, params object[] arguments) {
            _logger.Fatal(message, arguments);
            return Task.CompletedTask;
        }

        public Task Fatal(Exception ex, string message, params object[] arguments) {
            _logger.Fatal(ex, message, arguments);
            return Task.CompletedTask;
        }

        public Task Info(string message, params object[] arguments) {
            _logger.Info(message, arguments);
            return Task.CompletedTask;
        }

        public Task Info(Exception ex, string message, params object[] arguments) {
            _logger.Info(ex, message, arguments);
            return Task.CompletedTask;
        }

        public Task Warn(string message, params object[] arguments) {
            _logger.Warn(message, arguments);
            return Task.CompletedTask;
        }

        public Task Warn(Exception ex, string message, params object[] arguments) {
            _logger.Warn(ex, message, arguments);
            return Task.CompletedTask;
        }

        public Task Trace(string message, params object[] arguments) {
            _logger.Trace(message, arguments);
            return Task.CompletedTask;
        }

        public Task Trace(Exception ex, string message, params object[] arguments) {
            _logger.Trace(ex, message, arguments);
            return Task.CompletedTask;
        }
    }
}
