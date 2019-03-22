namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using MediatR;

    public class GetEventDetailQuery : IRequest<EventDetailModel>
    {
        public int Id { get; set; }
    }
}
