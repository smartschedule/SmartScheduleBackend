namespace SmartSchedule.Infrastucture.Authentication
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Application.Interfaces;
    using SmartSchedule.Application.Models;
    using SmartSchedule.Persistence;

    public class JwtService : IJwtService
    {
        private readonly SmartScheduleDbContext _context;
        private readonly IOptions<JwtSettings> _jwt;
        public JwtService(SmartScheduleDbContext context, IOptions<JwtSettings> jwt)
        {
            _context = context;
            _jwt = jwt;
        }
        public async Task<IActionResult> Login(EmailSignInModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(model.Email));
            if (user == null)
            {
                throw new NotFoundException(model.Email, -1);
            }
            else if (!PasswordHelper.ValidatePassword(model.Password, user.Password))
            {
                return new UnauthorizedResult();
            }

            return new ObjectResult(GenerateJwtToken(model.Email, user.Id, false));
        }

        private JwtTokenModel GenerateJwtToken(string email, int id, bool isAdmin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Value.Key);

            var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.UserData, id.ToString())
                });
            if (isAdmin)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                claims.AddClaim(new Claim(ClaimTypes.Role, "SuperAdmin"));
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
