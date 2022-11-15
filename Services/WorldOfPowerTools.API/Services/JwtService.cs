using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimsPrincipalExtensions.CLAIM_USER_GUID, user.Id.ToString()),
                new Claim(ClaimsPrincipalExtensions.CLAIM_USER_RIGHTS, user.Rights.ToString()),
            };

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
              issuer: _configuration["Jwt:Issuer"],
              audience: _configuration["Jwt:Audience"],
              notBefore: now,
              claims: claims,
              expires: now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
