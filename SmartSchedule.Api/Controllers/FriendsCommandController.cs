namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
            var command = new SendFriendRequestCommand
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                FriendId = friendId
            };

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/acceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody]int requestingUserId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var command = new AcceptFriendRequestCommand
            {
                RequestedUserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                RequestingUserId = requestingUserId
            };

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/rejectFriendRequest")]
        public async Task<IActionResult> RejectFriendRequest([FromBody]int requestingUserId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var command = new RejectFriendRequestCommand
            {
                RequestedUserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                RequestingUserId = requestingUserId
            };

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/removeFriend")]
        public async Task<IActionResult> RemoveFriend([FromBody]int friendId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var command = new RemoveFriendCommand
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                FriendId = friendId
            };

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/blockUser")]
        public async Task<IActionResult> BlockUser([FromBody]int userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var command = new BlockUserCommand
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                UserToBlock = userId
            };

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/unblockUser")]
        public async Task<IActionResult> UnblockUser([FromBody]int userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var command = new UnblockUserCommand
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                UserToUnblockId = userId
            };

            return Ok(await Mediator.Send(command));
        }
    }
}
