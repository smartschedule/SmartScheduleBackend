namespace SmartSchedule.Application.Calendar.Commands.CreateCalendar
{
    using FluentValidation;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.Helpers;

    public class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarRequest>
    {
        public CreateCalendarCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("You must set UserId.");
            RuleFor(x => x.UserId).MustAsync(async (request, val, token) =>
            {
                var userResult = await uow.UsersRepository.GetByIdAsync(val);

                if (userResult == null)
                {
                    return false;
                }

                return true;
            }).WithMessage("This user does not exist.");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.ColorHex).NotEmpty().WithMessage("ColorHex cannot be empty");
            RuleFor(x => x.ColorHex).Matches(ColorValidationHelper.HEX_RGB_REGEX).WithMessage("ColorHex must be in HEX.");
        }
    }
}
