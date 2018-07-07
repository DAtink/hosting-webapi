using Microsoft.AspNetCore.Mvc;
using TestLogics.Logics;
using TestService.Dto;

namespace TestService.Controllers
{
  [Route("api/[controller]")]
  public class TestController : Controller
  {
    private readonly ITestLogics _logics;
    private readonly IExternal _external;

    public TestController(ITestLogics logics, IExternal external)
    {
      _logics = logics;
      _external = external;
    }

    [HttpGet("")]
    public string Get()
    {
      return _logics.GetMessage();
    }

    [HttpPost("")]
    public TestForm Post([FromBody] TestForm form)
    {
      _external.ExternalFunction();
      return form;
    }
  }
}
