using SimpleInjector;
using SimpleInjector.Packaging;
using TestLogics.Logics;

namespace TestService
{
  public class TestLogicsPackage : IPackage
  {
    public void RegisterServices(Container container)
    {
      container.Register<ITestLogics, TestLogics.Logics.Impl.TestLogics>(Lifestyle.Singleton);
    }
  }
}
