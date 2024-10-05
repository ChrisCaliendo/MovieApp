using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Dto;
using System.Security.Claims;
using System.Text;

namespace MovieApp.Infastructure
{
    public sealed class TokenProvider(IConfiguration configuration)
    {
        public string Create(LoginDto user)
        {
            string secretKey = configuration["Jwt:Secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name.ToString()),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
