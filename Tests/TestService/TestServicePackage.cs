using SimpleInjector;
using SimpleInjector.Packaging;
using TestService.Logics;
using TestService.Logics.Impl;

namespace TestService
{
  public class TestServicePackage : IPackage
  {
    public void RegisterServices(Container container)
    {
      container.Register<ITestLogics, TestLogics>(Lifestyle.Singleton);
    }
  }
}
