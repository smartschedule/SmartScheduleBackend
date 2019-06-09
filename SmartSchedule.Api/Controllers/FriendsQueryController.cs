namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Friends.Queries.GetBlockedUsers;
    using SmartSchedule.Application.Friends.Queries.GetFriends;
    using SmartSchedule.Application.Friends.Queries.GetPendingUserFriendRequests;
    using SmartSchedule.Application.Friends.Queries.GetUserFriendRequests;

    public class FriendsQueryController : BaseController
    {
        #region Common
        [Authorize]
        [HttpGet("/api/user/getFriendsList")]
        public async Task<IActionResult> GetFriendsList()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));
            var command = new GetFriendsListQuery(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpGet("/api/user/getBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsersList()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));
            var command = new GetBlockedUsersListQuery(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpGet("/api/user/getPendingUserFriendRequests")]
        public async Task<IActionResult> GetPendingUserFriendRequests()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));
            var command = new GetPendingUserFriendRequestsQuery(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpGet("/api/user/getUsersFriendRequests")]
        public async Task<IActionResult> GetUserFriendRequests()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));
            var command = new GetUserFriendRequestsQuery(requestData);

            return Ok(await Mediator.Send(command));
        }
        #endregion

        #region Admin
        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/user/getFriendsList")]
        public async Task<IActionResult> AdminGetFriendsList([FromBody]IdRequest user)
        {
            return Ok(await Mediator.Send(new GetFriendsListQuery(user)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/user/getBlockedUsers")]
        public async Task<IActionResult> AdminGetBlockedUsersList([FromBody]IdRequest user)
        {
            return Ok(await Mediator.Send(new GetBlockedUsersListQuery(user)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/user/getPendingUserFriendRequests")]
        public async Task<IActionResult> AdminGetPendingUserFriendRequests([FromBody]IdRequest user)
        {
            return Ok(await Mediator.Send(new GetPendingUserFriendRequestsQuery(user)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/user/getUsersFriendRequests")]
        public async Task<IActionResult> AdminGetUserFriendRequests([FromBody]IdRequest user)
        {
            return Ok(await Mediator.Send(new GetUserFriendRequestsQuery(user)));
        }
        #endregion
    }
}
