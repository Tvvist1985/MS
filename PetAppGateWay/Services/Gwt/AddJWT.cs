using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetAppGateWay.Services.Gwt
{
    public class AddJWT
    {
        public const string ISSUER = "PetServer"; // издатель токена
        public const string AUDIENCE = "PetClient"; // потребитель токена
        const string KEY = "myCubykeymysupersecret_secretsecretsecretkey!123";   // ключ для шифрации для 256  должен быть не мение 32 байт

        //Method: Генерация ключа токина
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

        //Method:Создание токена
        public static string AddToken(in string id, in string role)
        {
            var claims = new List<Claim> { new Claim("Id", id), new Claim(ClaimTypes.Role, role) };//Создание клеймов

            var jwt = new JwtSecurityToken(
                    issuer: ISSUER,
                    audience: AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(3000000)), // время жизни токена
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
