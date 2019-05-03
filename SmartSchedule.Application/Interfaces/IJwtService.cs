namespace SmartSchedule.Application.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Authorization;

    public interface IJwtService
    {
        Task<IActionResult> Login(EmailSignInModel model);
    }
}
