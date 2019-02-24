using HostingWebapi.Infrastructure.Logging;
using TestLogics.Logics;

namespace TestService.External
{
  public class External : IExternal
  {
    private readonly ILogger _logger;

    public External(ILogger logger)
    {
      _logger = logger;
    }

    public void ExternalFunction()
    {
      var k = new object();
      _logger.LogInfo("k variable created");
    }
  }
}
