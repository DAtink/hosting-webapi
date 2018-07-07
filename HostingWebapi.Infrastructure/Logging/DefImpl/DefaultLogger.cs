using NLog;
using System;

namespace HostingWebapi.Infrastructure.Logging.DefImpl
{
  public class DefaultLogger : ILogger
  {
    private readonly Logger _logger;

    public DefaultLogger()
    {
      _logger = NLog.LogManager.GetLogger("HostingLogger");
    }

    public void LogError(Exception exception, string message)
    {
      _logger.Error(exception, message);
    }

    public void LogInfo(string message)
    {
      _logger.Info(message);
    }
  }
}
