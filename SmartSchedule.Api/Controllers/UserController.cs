using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartSchedule.Application.User.Commands.CreateUser;

namespace SmartSchedule.Api.Controllers
{
  
    public class UserController : BaseController
    {
        [HttpPost("/api/register")]
        public async Task<IActionResult> Registration([FromBody]CreateUserCommand user)
        {
            return Ok(await Mediator.Send(user));
        }
    }
}
