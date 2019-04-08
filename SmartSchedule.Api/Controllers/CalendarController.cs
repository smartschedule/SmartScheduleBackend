namespace SmartSchedule.Api.Controllers
{
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
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class CalendarController : BaseController
    {
        [Authorize]
        [HttpPost("/api/CreateCalendar")]
        public async Task<IActionResult> CreateCalendar([FromBody]CreateCalendarCommand calendar)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var command = new CreateCalendarCommand
            {
                UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value),
                Name = calendar.Name,
                ColorHex = calendar.ColorHex
            };

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/DeleteCalendar")]
        public async Task<IActionResult> DeleteCalendar([FromBody]DeleteCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [Authorize]
        [HttpPost("/api/UpdateCalendar")]
        public async Task<IActionResult> UpdateCalendar([FromBody]UpdateCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [Authorize]
        [HttpPost("/api/AddFriendToCalendar")]
        public async Task<IActionResult> AddFriendToCalendar([FromBody]AddFriendToCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [Authorize]
        [HttpPost("/api/DeleteFriendFromCalendar")]
        public async Task<IActionResult> DeleteFriendFromCalendar([FromBody]DeleteFriendFromCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [Authorize]
        [HttpPost("/api/DeleteEventsFromCalendar")]
        public async Task<IActionResult> DeleteEventsFromCalendar([FromBody]DeleteEventsFromCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
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
            var query = new GetCalendarDetailQuery
            {
                Id = id
            };

            return Ok(await Mediator.Send(query));
        }
    }
}