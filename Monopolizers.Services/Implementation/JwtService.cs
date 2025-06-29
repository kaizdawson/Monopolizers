using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Monopolizers.Repository.DB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Monopolizers.Service.Implementation
{


    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly CardARContext _context;

        public JwtService(IConfiguration config, CardARContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var plan = _context.PricingPlans.FirstOrDefault(p => p.PricingPlansId == user.PricingPlansId);
            var accessLevel = plan?.AccessLevel ?? "Basic";

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("accessLevel", accessLevel),
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
