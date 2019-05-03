namespace SmartSchedule.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.Event.Commands.CreateEvent;
    using SmartSchedule.Application.Event.Commands.DeleteEvent;
    using SmartSchedule.Application.Event.Commands.UpdateEvent;
    using SmartSchedule.Application.Event.Queries.GetEventDetails;
    using SmartSchedule.Application.Event.Queries.GetEventList;

    public class EventController : BaseController
    {
        [Authorize]
        [HttpPost("/api/CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

        [Authorize]
        [HttpPost("/api/UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody]UpdateEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

        [Authorize]
        [HttpPost("/api/DeleteEvent")]
        public async Task<IActionResult> DeleteEvent([FromBody]DeleteEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

        [Authorize]
        [HttpGet("/api/events")]
        public async Task<IActionResult> GetCalendarsList()
        {
            return Ok(await Mediator.Send(new GetEventListQuery()));
        }

        [Authorize]
        [HttpGet("/api/event/details/{id}")]
        public async Task<IActionResult> GetEventDetails(int id)
        {
            var query = new GetEventDetailQuery
            {
                Id = id
            };

            return Ok(await Mediator.Send(query));
        }

    }
}