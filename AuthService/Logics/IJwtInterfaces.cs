using Microsoft.IdentityModel.Tokens;

namespace AuthService.Logics
{
  // Ключ для создания подписи (приватный)
  public interface IJwtSigningEncodingKey
  {
    string SigningAlgorithm { get; }

    SecurityKey GetKey();
  }

  // Ключ для проверки подписи (публичный)
  public interface IJwtSigningDecodingKey
  {
    SecurityKey GetKey();
  }
}
