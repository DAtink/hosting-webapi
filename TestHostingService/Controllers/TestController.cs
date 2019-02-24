using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public string Get()
    {
      var nameIdentifier = this.HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

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
