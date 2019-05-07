namespace SmartSchedule.Application.Friends.Commands.SendFriendRequest
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Persistence;

    public class SendFriendRequestCommandValidator : AbstractValidator<SendFriendRequestRequest>
    {
        public SendFriendRequestCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.FriendId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var friendRequest = await context.Friends.FirstOrDefaultAsync(x =>
                                        (x.FirstUserId.Equals(val) && x.SecoundUserId.Equals(request.UserId)));

                if (friendRequest == null)
                {
                    return true;
                }

                return false;
            }).WithMessage("This user has already been requested to friends by second user");

            RuleFor(x => x.UserId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var friendRequest = await context.Friends.FirstOrDefaultAsync(x =>
                                         (x.FirstUserId.Equals(val) && x.SecoundUserId.Equals(request.FriendId)));

                if (friendRequest == null)
                {
                    return true;
                }

                return false;
            }).WithMessage("Request has already been sended");

        }
    }
}
