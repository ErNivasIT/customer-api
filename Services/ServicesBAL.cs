
using Customer_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Customer_API.Services
{
    public class ServicesBAL : IServicesBAL
    {
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly IConfiguration configuration;

        public ServicesBAL(TokenValidationParameters tokenValidationParameters,IConfiguration configuration)
        {
            this.tokenValidationParameters = tokenValidationParameters;
            this.configuration = configuration;
        }
        public async Task<string> GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = tokenValidationParameters.IssuerSigningKey,
                ValidateIssuer = tokenValidationParameters.ValidateIssuer,
                ValidIssuer = tokenValidationParameters.ValidIssuer,
                ValidateAudience = tokenValidationParameters.ValidateAudience,
                ValidAudience = tokenValidationParameters.ValidAudience,
                ValidateLifetime = false // We are validating the token after its expiration, so we don't need to check the lifetime
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<string> GenerateAccessToken(string? name)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = configuration.GetSection("JwtSecurityToken:Key").Value;
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "SuperAdmin"),
                    // Add more claims as needed
                }),
                Issuer = configuration.GetSection("JwtSecurityToken:Issuer").Value,
                Audience = configuration.GetSection("JwtSecurityToken:Audience").Value,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task Update(string? name, string newRefreshToken)
        {
            await Task.CompletedTask;
        }
    }
}
