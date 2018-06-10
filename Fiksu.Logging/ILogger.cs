using System;
using System.Threading.Tasks;

namespace Fiksu.Logging
{
    public interface ILogger
    {
        Task Debug(string message, params object[] arguments);
        Task Debug(Exception ex, string message, params object[] arguments);

        Task Error(string message, params object[] arguments);
        Task Error(Exception ex, string message, params object[] arguments);

        Task Fatal(string message, params object[] arguments);
        Task Fatal(Exception ex, string message, params object[] arguments);

        Task Info(string message, params object[] arguments);
        Task Info(Exception ex, string message, params object[] arguments);

        Task Trace(string message, params object[] arguments);
        Task Trace(Exception ex, string message, params object[] arguments);

        Task Warn(string message, params object[] arguments);
        Task Warn(Exception ex, string message, params object[] arguments);
    }
}
