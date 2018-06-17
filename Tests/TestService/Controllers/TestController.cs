using Microsoft.AspNetCore.Mvc;
using TestService.Logics;

namespace TestService.Controllers
{
  [Route("api/[controller]")]
  public class TestController : Controller
  {
    private readonly ITestLogics _logics;

    public TestController(ITestLogics logics)
    {
      _logics = logics;
    }

    // GET api/values
    [HttpGet("")]
    public string Get()
    {
      return _logics.GetMessage();
    }
  }
}
