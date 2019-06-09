namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Event.Commands.CreateEvent;
    using SmartSchedule.Application.Event.Commands.DeleteEvent;
    using SmartSchedule.Application.Event.Commands.UpdateEvent;
    using SmartSchedule.Application.Event.Queries.GetCalendarEvents;
    using SmartSchedule.Application.Event.Queries.GetEventDetails;
    using SmartSchedule.Application.Event.Queries.GetEvents;
    using SmartSchedule.Application.Event.Queries.GetUserEvents;

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
        public async Task<IActionResult> GetUserEvents()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var data = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));

            return Ok(await Mediator.Send(new GetUserEventsQuery(data)));
        }

        [Authorize]
        [HttpGet("/api/calendar/events")]
        public async Task<IActionResult> GetCalendarEvents([FromBody]IdRequest eventRequest)
        {
            return Ok(await Mediator.Send(new GetCalendarEventsQuery(eventRequest)));
        }

        [Authorize]
        [HttpGet("/api/event/details/{id}")]
        public async Task<IActionResult> GetEventDetails(int id)
        {
            var query = new GetEventDetailsQuery(new IdRequest(id));
            return Ok(await Mediator.Send(query));
        }
        #endregion

        #region Admin
        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/events")]
        public async Task<IActionResult> AdminGetEvents()
        {
            return Ok(await Mediator.Send(new GetEventsQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/user/events")]
        public async Task<IActionResult> AdminGetUserEvents([FromBody]IdRequest eventRequest)
        {
            return Ok(await Mediator.Send(new GetUserEventsQuery(eventRequest)));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/calendar/events")]
        public async Task<IActionResult> AdminGetCalendarEvents([FromBody]IdRequest eventRequest)
        {
            return Ok(await Mediator.Send(new GetCalendarEventsQuery(eventRequest)));
        }
        #endregion
    }
}