using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HostingWebapi.Infrastructure
{
  public class HostBuilder
  {
    private readonly Assembly _serviceAssembly;

    private IHostingEnvironment HostingEnvironment { get; set; }
    private IConfiguration Configuration { get; set; }

    protected HostBuilder(Assembly serviceAssembly)
    {
      _serviceAssembly = serviceAssembly;
    }

    public static HostBuilder Create()
    {
      return new HostBuilder(Assembly.GetCallingAssembly());
    }

    private void ConfigureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
    {
      HostingEnvironment = hostingContext.HostingEnvironment;
      Configuration = config.Build();
    }

    private void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc()
              .AddApplicationPart(_serviceAssembly)
              .AddControllersAsServices();
    }

    private void ConfigureApp(IApplicationBuilder app)
    {
      app.UseMvcWithDefaultRoute();
    }

    public IWebHost BuildWebHost(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)
          .UseKestrel()
          .ConfigureAppConfiguration(ConfigureAppConfiguration)
          .ConfigureServices(ConfigureServices)
          .Configure(ConfigureApp)
          .Build();
    }
  }
}
