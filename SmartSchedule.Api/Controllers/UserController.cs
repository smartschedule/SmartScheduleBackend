namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Application.DTO.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Application.User.Commands.CreateUser;
    using SmartSchedule.Application.User.Commands.UpdateUser;
    using SmartSchedule.Application.User.Queries.GetUserDetails;
    using SmartSchedule.Application.User.Queries.GetUsers;

    public class UserController : BaseController
    {
        [HttpPost("/api/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]GetResetPasswordTokenQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("/api/resetPassword/{token}")]
        public async Task<IActionResult> ResetPassword(string token, [FromBody]ResetPasswordCommand request)
        {
            request.Token = token;

            return Ok(await Mediator.Send(request));
        }

        #region Common
        [HttpPost("/api/register")]
        public async Task<IActionResult> Registration([FromBody]CreateUserRequest user)
        {
            return Ok(await Mediator.Send(new CreateUserCommand(user)));
        }

        [Authorize(Roles = "User")]
        [HttpGet("/api/user/details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var data = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));

            return Ok(await Mediator.Send(new GetUserDetailsQuery(data)));
        }

        [Authorize]
        [HttpPost("/api/user/update")]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserRequest user)
        {
            return Ok(await Mediator.Send(new UpdateUserCommand(user)));
        }

        [Authorize]
        [HttpGet("/api/friend/details/{id}")]
        [HttpGet("/api/user/details/{id}")]
        [HttpGet("/api/admin/user/details/{id}")]
        public async Task<IActionResult> GetFriendDetails(int id)
        {
            var query = new GetUserDetailsQuery(new IdRequest(id));

            return Ok(await Mediator.Send(query));
        }

        [Authorize]
        [HttpGet("/api/users")]
        public async Task<IActionResult> GetUsersList()
        {
            return Ok(await Mediator.Send(new GetUsersQuery()));
        }
        #endregion
    }
}
