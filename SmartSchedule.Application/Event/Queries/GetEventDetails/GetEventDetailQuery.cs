namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using MediatR;
    using SmartSchedule.Application.Event.Models;

    public class GetEventDetailQuery : IRequest<EventDetailModel>
    {
        public int Id { get; set; }
    }
}
