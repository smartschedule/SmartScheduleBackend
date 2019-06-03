namespace SmartSchedule.Application.DAL.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Authentication;

    public interface IJwtService
    {
        JwtTokenModel GenerateJwtToken(string email, int id, bool isAdmin);
    }
}
