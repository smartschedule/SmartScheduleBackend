namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Friends.Commands.AcceptFriendRequest;
    using SmartSchedule.Application.Friends.Commands.BlockUser;
    using SmartSchedule.Application.Friends.Commands.RejectFriendRequest;
    using SmartSchedule.Application.Friends.Commands.RemoveFriend;
    using SmartSchedule.Application.Friends.Commands.SendFriendRequest;
    using SmartSchedule.Application.Friends.Commands.UnblockUser;

    public class FriendsCommandController : BaseController
    {
        [Authorize]
        [HttpPost("/api/user/friendRequest")]
        public async Task<IActionResult> CreateFriendRequest([FromBody]int friendId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new SendFriendRequestRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                FriendId = friendId
            };
            var command = new SendFriendRequestCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/acceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody]int requestingUserId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new AcceptOrRejectFriendRequestRequest
            {
                RequestedUserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                RequestingUserId = requestingUserId
            };
            var command = new AcceptFriendRequestCommand(requestData);      

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/rejectFriendRequest")]
        public async Task<IActionResult> RejectFriendRequest([FromBody]int requestingUserId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new AcceptOrRejectFriendRequestRequest
            {
                RequestedUserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                RequestingUserId = requestingUserId
            };
            var command = new RejectFriendRequestCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/removeFriend")]
        public async Task<IActionResult> RemoveFriend([FromBody]int friendId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new RemoveFriendRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                FriendId = friendId
            };
            var command = new RemoveFriendCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/blockUser")]
        public async Task<IActionResult> BlockUser([FromBody]int userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new BlockUserRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                UserToBlock = userId
            };
            var command = new BlockUserCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/unblockUser")]
        public async Task<IActionResult> UnblockUser([FromBody]int userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new UnblockUserRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                UserToUnblockId = userId
            };
            var command = new UnblockUserCommand(requestData);

            return Ok(await Mediator.Send(command));
        }
    }
}
