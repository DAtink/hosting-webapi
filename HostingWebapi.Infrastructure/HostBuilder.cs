using HostingWebapi.Infrastructure.Middleware;
using HostingWebapi.Infrastructure.SimpleInjector;
using HostingWebapi.Infrastructure.Swagger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HostingWebapi.Infrastructure
{
  public class HostBuilder
  {
    private readonly Container _container;
    private readonly Assembly _serviceAssembly;

    private IHostingEnvironment HostingEnvironment { get; set; }
    private IConfiguration Configuration { get; set; }

    protected HostBuilder(Assembly serviceAssembly)
    {
      _serviceAssembly = serviceAssembly;
      _container = new Container();
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
              .AddApplicationPart(_serviceAssembly);

      services.AddSwagger();

      services.IntegrateSimpleInjector(_container);
    }

    private void ConfigureApp(IApplicationBuilder app)
    {
      app.InitializeContainer(_container, _serviceAssembly);

      // NOTE: Add custom middleware
      app.UseMiddleware<LoggingMiddleware>(_container);

      _container.Verify();

      // ASP.NET default stuff here
      app.UseMvcWithDefaultRoute();
      app.UseStaticFiles();

      // Swagger
      app.ConfigureSwagger();
    }

    public IWebHost BuildWebHost(string[] args)
    {
      var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
      var pathToContentRoot = Path.GetDirectoryName(pathToExe);

      return WebHost.CreateDefaultBuilder(args)
          .UseKestrel()
          .UseContentRoot(pathToContentRoot)
          .ConfigureAppConfiguration(ConfigureAppConfiguration)
          .ConfigureServices(ConfigureServices)
          .Configure(ConfigureApp)
          .Build();
    }

    public void BuildAndRun(string[] args)
    {
      var webHost = BuildWebHost(args);
      if (args.Contains("--install"))
      {
        webHost.RunAsService();
        return;
      }

      webHost.Run();
    }
  }
}
