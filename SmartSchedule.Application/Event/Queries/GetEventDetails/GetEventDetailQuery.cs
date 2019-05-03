namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using MediatR;
    using SmartSchedule.Application.DTO.Event;

    public class GetEventDetailQuery : IRequest<EventDetailModel>
    {
        public int Id { get; set; }
    }
}
