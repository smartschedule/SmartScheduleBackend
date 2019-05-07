namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using FluentValidation;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Persistence;

    public class BlockUserCommandValidator : AbstractValidator<BlockUserRequest>
    {
        public BlockUserCommandValidator(SmartScheduleDbContext context)
        {

        }
    }
}
