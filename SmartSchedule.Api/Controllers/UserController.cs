using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchedule.Application.User.Commands.CreateUser;
using SmartSchedule.Application.User.Queries.GetUserDetails;

namespace SmartSchedule.Api.Controllers
{
  
    public class UserController : BaseController
    {
        [HttpPost("/api/register")]
        public async Task<IActionResult> Registration([FromBody]CreateUserCommand user)
        {
            return Ok(await Mediator.Send(user));
        }

        [Authorize]
        [HttpGet("/api/user/details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var query = new GetUserDetailQuery
            {
                Id = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value)
            };

            return Ok(await Mediator.Send(query));
        }
    }
}
