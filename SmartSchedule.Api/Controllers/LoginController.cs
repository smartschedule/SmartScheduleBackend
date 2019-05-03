namespace SmartSchedule.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Application.Interfaces;

    public class LoginController : BaseController
    {
        private readonly IJwtService _jwt;

        public LoginController(IJwtService jwt)
        {
            _jwt = jwt;
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest model)
        {
            return await _jwt.Login(model);
        }
    }
}
