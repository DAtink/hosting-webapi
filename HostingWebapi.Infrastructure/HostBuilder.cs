using HostingWebapi.Infrastructure.Middleware;
using HostingWebapi.Infrastructure.SimpleInjector;
using HostingWebapi.Infrastructure.Swagger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimpleInjector;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

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

      const string jwtSchemeName = "JwtBearer";
      services
          .AddAuthentication(options => {
            options.DefaultAuthenticateScheme = jwtSchemeName;
            options.DefaultChallengeScheme = jwtSchemeName;
          })
          .AddJwtBearer(jwtSchemeName, jwtBearerOptions => {
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IcdbWillNeverDie")),

              ValidateIssuer = true,
              ValidIssuer = "DemoApp",

              ValidateAudience = true,
              ValidAudience = "DemoAppClient",

              ValidateLifetime = true,
              ClockSkew = TimeSpan.FromSeconds(5)
            };
          });

      services.IntegrateSimpleInjector(_container);
    }

    private void ConfigureApp(IApplicationBuilder app)
    {
      app.InitializeContainer(_container, _serviceAssembly);

      // NOTE: Add custom middleware
      app.UseMiddleware<LoggingMiddleware>(_container);

      _container.Verify();

      app.UseAuthentication();

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
          .UseKestrel(o => o.Listen(IPAddress.Loopback, 5005))
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
