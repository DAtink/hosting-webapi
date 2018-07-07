using System;

namespace HostingWebapi.Infrastructure.Logging
{
  public interface ILogger
  {
    void LogInfo(string message);
    void LogError(Exception exception, string message);
  }
}
