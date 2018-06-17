﻿using SimpleInjector;
using SimpleInjector.Packaging;
using TestLogics.Logics;

namespace TestService
{
  public class TestServicePackage : IPackage
  {
    public void RegisterServices(Container container)
    {
      container.Register<IExternal, External.External>(Lifestyle.Singleton);
    }
  }
}
