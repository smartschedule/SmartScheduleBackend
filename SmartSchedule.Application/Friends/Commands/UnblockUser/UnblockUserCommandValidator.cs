namespace SmartSchedule.Application.Friends.Commands.UnblockUser
{
    using FluentValidation;
    using SmartSchedule.Persistence;

    public class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
    {
        public UnblockUserCommandValidator(SmartScheduleDbContext context)
        {

        }
    }
}
