﻿namespace SmartSchedule.Application.Friends.Commands.BlockUser
{
    using FluentValidation;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Friends.Commands;

    public class BlockUserCommandValidator : AbstractValidator<BlockUserRequest>
    {
        public BlockUserCommandValidator(IUnitOfWork uow)
        {

        }
    }
}
