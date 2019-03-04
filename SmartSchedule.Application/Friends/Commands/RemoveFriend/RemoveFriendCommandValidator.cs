using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace SmartSchedule.Application.Friends.Commands.RemoveFriendRequest
{
    public class RemoveFriendCommandValidator : AbstractValidator<RemoveFriendCommand>
    {
        public RemoveFriendCommandValidator()
        {

        }
    }
}
