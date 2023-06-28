using Microsoft.IdentityModel.Tokens;
using Portfolio.Backend.Csharp.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.Backend.Csharp.Configs
{
    public class JwtAuthenticationManager
    {
        private readonly byte[] _key;

        public JwtAuthenticationManager(byte[] key)
        {
            _key = key;
        }

        public string GenerateToken(string userId, Role role)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            //var tokenKey = Encoding.ASCII.GetBytes(_key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.UserData, userId),
                    new Claim(ClaimTypes.Role, role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = "SaneManiacWorks",
                Audience = "PortfolioFrontend"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
