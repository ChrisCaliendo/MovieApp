using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Dto;
using System.Security.Claims;
using System.Text;

namespace MovieApp.Infastructure
{
    /// <summary>
    /// A provider for creating JWT tokens used for authentication and authorization.
    /// </summary>
    public sealed class TokenProvider(IConfiguration configuration)
    {
        /// <summary>
        /// Creates a JWT token for the specified user with the necessary claims, signing credentials, expiration time, issuer, and audience.
        /// </summary>
        /// <param name="user">The user object containing the details for the token creation (like the user's name).</param>
        /// <returns>A JWT token as a string.</returns>
        public string Create(LoginDto user)
        {
            string secretKey = configuration["Jwt:Secret"]; // Retrieve the secret key from the application's configuration to sign the token.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Create a symmetric security key using the secret key. 
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); 

            // Define the token descriptor with claims, expiration time, signing credentials, issuer, and audience.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Set the subject claim with the user's name (JWT registered 'sub' claim).
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name.ToString()), // User's name as 'sub' claim.
                ]),

                // Set the expiration time of the token (1 hour from the current UTC time).
                Expires = DateTime.UtcNow.AddMinutes(60),

                // Assign signing credentials and other properties of the token like issuer and audience.
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"], // The token's issuer (usually the application).
                Audience = configuration["Jwt:Audience"] // The intended recipient of the token (audience).
            };

            // Create a token handler to generate the token from the descriptor.
            var handler = new JsonWebTokenHandler();

            // Generate the token and return it as a string.
            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
