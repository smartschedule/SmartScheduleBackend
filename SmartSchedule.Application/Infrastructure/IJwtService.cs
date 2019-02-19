namespace SmartSchedule.Application.Infrastructure
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.Models;

    public interface IJwtService
    {
        Task<IActionResult> Login(EmailSignInModel model);
    }
}
