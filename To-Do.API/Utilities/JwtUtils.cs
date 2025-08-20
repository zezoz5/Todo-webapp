using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace To_Do.API.Utilities
{
    public class JwtUtils
    {
        public static string GenerateJwt(string username, IdentityUser user, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!);
            var issuer = config["JwtSettings:Issuer"];
            var audience = config["JwtSettings:Audience"];

            var roles = userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(config.GetValue<int>("JwtSettings:DurationInMinutes")),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}