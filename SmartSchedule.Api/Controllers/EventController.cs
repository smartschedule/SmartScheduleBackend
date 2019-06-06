namespace SmartSchedule.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Event.Commands.CreateEvent;
    using SmartSchedule.Application.Event.Commands.DeleteEvent;
    using SmartSchedule.Application.Event.Commands.UpdateEvent;
    using SmartSchedule.Application.Event.Queries.GetEventDetails;
    using SmartSchedule.Application.Event.Queries.GetEventList;

    public class EventController : BaseController
    {
        #region Common
        [Authorize]
        [HttpPost("/api/event/create")]
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventRequest eventRequest)
        {
            return Ok(await Mediator.Send(new CreateEventCommand(eventRequest)));
        }

        [Authorize]
        [HttpPost("/api/event/update")]
        public async Task<IActionResult> UpdateEvent([FromBody]UpdateEventRequest eventRequest)
        {
            return Ok(await Mediator.Send(new UpdateEventCommand(eventRequest)));
        }

        [Authorize]
        [HttpPost("/api/event/delete")]
        public async Task<IActionResult> DeleteEvent([FromBody]IdRequest eventRequest)
        {
            return Ok(await Mediator.Send(new DeleteEventCommand(eventRequest)));
        }

        [Authorize]
        [HttpGet("/api/events")]
        public async Task<IActionResult> GetEventsList()
        {
            return Ok(await Mediator.Send(new GetEventListQuery()));
        }

        [Authorize]
        [HttpGet("/api/event/details/{id}")]
        public async Task<IActionResult> GetEventDetails(int id)
        {
            var query = new GetEventDetailQuery(new IdRequest(id));
            return Ok(await Mediator.Send(query));
        }
        #endregion
    }
}