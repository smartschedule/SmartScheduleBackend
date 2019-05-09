﻿namespace SmartSchedule.Application.User.Commands.UpdateUser
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.User.Commands;
    using SmartSchedule.Persistence;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserRequest>
    {
        private const int MIN_PASSWORD_LENGTH = 8;

        public UpdateUserCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("You must set username");
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MustAsync(async (request, val, token) =>
            {
                var userResult = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (userResult == null || userResult.Email.Equals(val))
                {
                    return true;
                }

                return false;
            }).WithMessage("This email is already in use.");
            RuleFor(x => x.Password).MinimumLength(MIN_PASSWORD_LENGTH).NotEmpty();
        }
    }
}
