namespace SmartSchedule.Api.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SmartSchedule.Application.Calendar.Commands.AddFriendToCalendar;
    using SmartSchedule.Application.Calendar.Commands.CreateCalendar;
    using SmartSchedule.Application.Calendar.Commands.DeleteCalendar;
    using SmartSchedule.Application.Calendar.Commands.DeleteEventsFromCalendar;
    using SmartSchedule.Application.Calendar.Commands.DeleteFriendFromCalendar;
    using SmartSchedule.Application.Calendar.Commands.UpdateCalendar;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarDetails;
    using SmartSchedule.Application.Calendar.Queries.GetCalendarList;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.DTO.Common;

    public class CalendarController : BaseController
    {
        [Authorize]
        [HttpPost("/api/CreateCalendar")]
        public async Task<IActionResult> CreateCalendar([FromBody]CreateCalendarRequest calendar)
        {
            var command = new CreateCalendarCommand(calendar);

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            calendar.UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/DeleteCalendar")]
        public async Task<IActionResult> DeleteCalendar([FromBody]IdRequest calendar)
        {
            var command = new DeleteCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/UpdateCalendar")]
        public async Task<IActionResult> UpdateCalendar([FromBody]UpdateCalendarRequest calendar)
        {
            var command = new UpdateCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/AddFriendToCalendar")]
        public async Task<IActionResult> AddFriendToCalendar([FromBody]AddFriendToCalendarRequest calendar)
        {
            var command = new AddFriendToCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/DeleteFriendFromCalendar")]
        public async Task<IActionResult> DeleteFriendFromCalendar([FromBody]DeleteFriendFromCalendarRequest calendar)
        {
            var command = new DeleteFriendFromCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/DeleteEventsFromCalendar")]
        public async Task<IActionResult> DeleteEventsFromCalendar([FromBody]IdRequest calendar)
        {
            var command = new DeleteEventsFromCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpGet("/api/calendars")]
        public async Task<IActionResult> GetCalendarsList()
        {
            return Ok(await Mediator.Send(new GetCalendarsListQuery()));
        }

        [Authorize]
        [HttpGet("/api/calendar/details/{id}")]
        public async Task<IActionResult> GetCalendarDetails(int id)
        {
            var query = new GetCalendarDetailQuery(new IdRequest(id));

            return Ok(await Mediator.Send(query));
        }
    }
}