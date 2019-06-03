namespace SmartSchedule.Infrastucture.Authentication
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using SmartSchedule.Application.DAL.Interfaces;
    using SmartSchedule.Application.DTO.Authentication;

    public class JwtService : IJwtService
    {
        private readonly IOptions<JwtSettings> _settings;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings;
        }

        public JwtTokenModel GenerateJwtToken(string email, int id, bool isAdmin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Value.Key);

            var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.UserData, id.ToString())
                });
            if (isAdmin)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var result = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtTokenModel { Token = tokenHandler.WriteToken(result), ValidTo = result.ValidTo };
        }
    }
}
