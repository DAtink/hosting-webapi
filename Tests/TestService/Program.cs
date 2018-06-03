using HostingWebapi.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace TestService
{
  class Program
  {
    static void Main(string[] args)
    {
      HostBuilder.Create().BuildWebHost(args).Run();
    }
  }
}
