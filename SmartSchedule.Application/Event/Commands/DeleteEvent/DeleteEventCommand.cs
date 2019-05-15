﻿namespace SmartSchedule.Application.Event.Commands.DeleteEvent
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.DAL.Interfaces.UoW;

    public class DeleteEventCommand : IRequest
    {
        public IdRequest Data { get; set; }

        public DeleteEventCommand()
        {

        }

        public DeleteEventCommand(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<DeleteEventCommand, Unit>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var eventE = await _uow.Events.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

                if (eventE == null)
                {
                    throw new NotFoundException("Event", data.Id);
                }

                _uow.Events.Remove(eventE);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
