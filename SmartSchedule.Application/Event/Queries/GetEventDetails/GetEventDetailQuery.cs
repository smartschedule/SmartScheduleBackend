namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using MediatR;
    using SmartSchedule.Application.DTO.Event.Commands;

    public class GetEventDetailQuery : IRequest<UpdateEventRequest>
    {
        public int Id { get; set; }
    }
}
