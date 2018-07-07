using HostingWebapi.Infrastructure;

namespace TestService
{
  class Program
  {
    static void Main(string[] args)
    {
      HostBuilder.Create().BuildAndRun(args);
    }
  }
}
