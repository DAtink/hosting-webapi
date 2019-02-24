using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Data;
using AuthService.Logics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IJwtSigningEncodingKey _signingEncodingKey;

    public AuthController(IJwtSigningEncodingKey signingEncodingKey)
    {
      _signingEncodingKey = signingEncodingKey;
    }

    [AllowAnonymous]
    [Route("")]
    [HttpPost]
    public ActionResult<string> Post(Credentials authRequest)
    {
      // 1. Проверяем данные пользователя из запроса.
      // ...

      // 2. Создаем утверждения для токена.
      var claims = new Claim[]
      {
            new Claim(ClaimTypes.NameIdentifier, authRequest.Name)
      };

      // 3. Генерируем JWT.
      var token = new JwtSecurityToken(
          issuer: "DemoApp",
          audience: "DemoAppClient",
          claims: claims,
          expires: DateTime.Now.AddMinutes(5),
          signingCredentials: new SigningCredentials(
                  _signingEncodingKey.GetKey(),
                  _signingEncodingKey.SigningAlgorithm)
      );

      string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
      return jwtToken;
    }

    [HttpGet("")]
    [Authorize]
    public string Get()
    {
      var nameIdentifier = this.HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

      return "";
    }
  }
}