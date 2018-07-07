using HostingWebapi.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HostingWebapi.Infrastructure.Middleware
{
  public class LoggingMiddleware : IMiddleware
  {
    private readonly ILogger _logger;

    public LoggingMiddleware(ILogger logger)
    {
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      var request = context.Request;
      _logger.LogInfo($"{request.Path}");

      await next(context);
    }
  }
}
