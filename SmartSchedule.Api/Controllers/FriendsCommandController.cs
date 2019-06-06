namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Application.Friends.Commands.AcceptFriendInvitation;
    using SmartSchedule.Application.Friends.Commands.BlockUser;
    using SmartSchedule.Application.Friends.Commands.RejectFriendRequest;
    using SmartSchedule.Application.Friends.Commands.RemoveFriend;
    using SmartSchedule.Application.Friends.Commands.SendFriendInvitation;
    using SmartSchedule.Application.Friends.Commands.UnblockUser;

    public class FriendsCommandController : BaseController
    {
        #region Common
        [Authorize]
        [HttpPost("/api/user/friendRequest")]
        public async Task<IActionResult> CreateFriendRequest([FromBody]IdRequest friend)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new SendFriendInvitationRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                FriendId = friend.Id
            };
            var command = new SendFriendInvitationCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/acceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody]IdRequest requestingUser)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new AcceptOrRejectFriendInvitationRequest
            {
                RequestedUserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                RequestingUserId = requestingUser.Id
            };
            var command = new AcceptFriendInvitationCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/rejectFriendRequest")]
        public async Task<IActionResult> RejectFriendRequest([FromBody]IdRequest requestingUser)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new AcceptOrRejectFriendInvitationRequest
            {
                RequestedUserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                RequestingUserId = requestingUser.Id
            };
            var command = new RejectFriendRequestCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/removeFriend")]
        public async Task<IActionResult> RemoveFriend([FromBody]IdRequest friend)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new RemoveFriendRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                FriendId = friend.Id
            };
            var command = new RemoveFriendCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/blockUser")]
        public async Task<IActionResult> BlockUser([FromBody]IdRequest user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new BlockUserRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                UserToBlock = user.Id
            };
            var command = new BlockUserCommand(requestData);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/user/unblockUser")]
        public async Task<IActionResult> UnblockUser([FromBody]IdRequest user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var requestData = new UnblockUserRequest
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                UserToUnblockId = user.Id
            };
            var command = new UnblockUserCommand(requestData);

            return Ok(await Mediator.Send(command));
        }
        #endregion
    }
}
