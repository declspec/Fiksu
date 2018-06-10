namespace Fiksu.Logging
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger(string name)
        {
            return new NLogLogger(NLog.LogManager.GetLogger(name));
        }
    }
}
