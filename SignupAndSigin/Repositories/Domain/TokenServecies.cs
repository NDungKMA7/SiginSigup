using Microsoft.IdentityModel.Tokens;
using SignupAndSigin.Models.DTO;
using SignupAndSigin.Repositories.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SignupAndSigin.Repositories.Domain
{
    public class TokenServecies : ITokenService
    {
        private readonly IConfiguration _configuration; 
        public TokenServecies(IConfiguration configuration)
        {
            _configuration = configuration;


        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters { 
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false,
            
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken security;
            var principal = tokenHandler.ValidateToken(token,tokenValidationParameters, out security);
            var jwtSecurityToken = security as JwtSecurityToken;
            if(jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase) ){
                throw new SecurityTokenException("Invalid token");
            }
            return principal;   

        }

        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var rng = RandomNumberGenerator.Create()) { 
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);    

            }
        }

        public TokenResponse getToken(IEnumerable<Claim> claims) 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);      
            return new TokenResponse { TokenString = tokenString, ValidTo = token.ValidTo };
        }

       

    }
}
