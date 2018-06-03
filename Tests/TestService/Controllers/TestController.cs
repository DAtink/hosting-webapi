using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TestService.Controllers
{
  [Route("api/[controller]")]
  public class TestController : Controller
  {
    // GET api/values
    [HttpGet("")]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }
  }
}
