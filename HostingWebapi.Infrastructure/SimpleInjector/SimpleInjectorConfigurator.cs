using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System.Reflection;
using System.Collections.Generic;

namespace HostingWebapi.Infrastructure.SimpleInjector
{
  public static class SimpleInjectorConfigurator
  {
    public static void IntegrateSimpleInjector(this IServiceCollection services, Container container)
    {
      container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddSingleton<IControllerActivator>(
          new SimpleInjectorControllerActivator(container));

      services.EnableSimpleInjectorCrossWiring(container);
      services.UseSimpleInjectorAspNetRequestScoping(container);
    }

    public static void InitializeContainer(this IApplicationBuilder app, Container container, Assembly rootAssembly)
    {
      // Add application presentation components:
      container.RegisterMvcControllers(app);
      container.RegisterMvcViewComponents(app);

      // NOTE: Add application services. For instance:
      container.RegisterPackages(new List<Assembly>() { rootAssembly });

      // Allow Simple Injector to resolve services from ASP.NET Core.
      container.AutoCrossWireAspNetComponents(app);
    }
  }
}
