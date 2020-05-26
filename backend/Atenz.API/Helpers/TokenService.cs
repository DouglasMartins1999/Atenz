using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Atenz.Domain.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Atenz.API.Helpers
{
    public class TokenService
    {
        public static string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var descriptor = new SecurityTokenDescriptor 
            {
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.isAdmin ? "Admin" : "CommonUser")
                }),
                
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}