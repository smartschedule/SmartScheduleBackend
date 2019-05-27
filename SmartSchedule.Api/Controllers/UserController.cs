namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Application.DTO.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.User.Commands.CreateUser;
    using SmartSchedule.Application.User.Commands.UpdateUser;
    using SmartSchedule.Application.User.Queries.GetUserDetails;
    using SmartSchedule.Application.User.Queries.GetUserList;

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
            var data = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));
            var query = new GetUserDetailQuery(data);

            return Ok(await Mediator.Send(query));
        }

        [Authorize]
        [HttpPost("/api/user/update")]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserCommand userCommand)
        {
            return Ok(await Mediator.Send(userCommand));
        }

        [Authorize]
        [HttpGet("/api/friend/details/{id}")]
        public async Task<IActionResult> GetFriendDetails(int id)
        {
            var query = new GetUserDetailQuery(new IdRequest(id));

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("/api/users")]
        public async Task<IActionResult> GetUsersList()
        {
            return Ok(await Mediator.Send(new GetUsersListQuery()));
        }
    }
}
