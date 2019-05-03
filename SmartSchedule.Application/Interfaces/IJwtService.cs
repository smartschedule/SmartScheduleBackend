namespace SmartSchedule.Application.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Authentication;

    public interface IJwtService
    {
        Task<IActionResult> Login(LoginRequest model);
    }
}
