namespace SmartSchedule.Infrastucture.Authentication
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.Infrastructure;
    using SmartSchedule.Application.Models;
    using SmartSchedule.Persistence;

    public class JwtService : IJwtService
    {
        private readonly SmartScheduleDbContext _context;
        public JwtService(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Login(EmailSignInModel model)
        {
            throw new NotImplementedException();
        }
    }
}
