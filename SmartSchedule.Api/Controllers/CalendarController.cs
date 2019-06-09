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
    using SmartSchedule.Application.Calendar.Queries.GetCalendars;
    using SmartSchedule.Application.Calendar.Queries.GetUserCalendars;
    using SmartSchedule.Application.DTO.Calendar.Commands;
    using SmartSchedule.Application.DTO.Common;

    public class CalendarController : BaseController
    {
        #region Common
        [Authorize]
        [HttpPost("/api/calendar/create")]
        public async Task<IActionResult> CreateCalendar([FromBody]CreateCalendarRequest calendar)
        {
            var command = new CreateCalendarCommand(calendar);

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            calendar.UserId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/calendar/delete")]
        public async Task<IActionResult> DeleteCalendar([FromBody]IdRequest calendar)
        {
            var command = new DeleteCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/calendar/update")]
        public async Task<IActionResult> UpdateCalendar([FromBody]UpdateCalendarRequest calendar)
        {
            var command = new UpdateCalendarCommand(calendar);

            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPost("/api/calendar/addFriend")]
        public async Task<IActionResult> AddFriendToCalendar([FromBody]AddFriendToCalendarRequest calendar)
        {
            return Ok(await Mediator.Send(new AddFriendToCalendarCommand(calendar)));
        }

        [Authorize]
        [HttpPost("/api/calendar/deleteFriend")]
        public async Task<IActionResult> DeleteFriendFromCalendar([FromBody]DeleteFriendFromCalendarRequest calendar)
        {
            return Ok(await Mediator.Send(new DeleteFriendFromCalendarCommand(calendar)));
        }

        [Authorize]
        [HttpPost("/api/calendar/deleteEvents")]
        public async Task<IActionResult> DeleteEventsFromCalendar([FromBody]IdRequest calendar)
        {
            return Ok(await Mediator.Send(new DeleteEventsFromCalendarCommand(calendar)));
        }

        [Authorize]
        [HttpGet("/api/calendars")]
        public async Task<IActionResult> GetCalendars()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = new IdRequest(int.Parse(identity.FindFirst(ClaimTypes.UserData).Value));

            return Ok(await Mediator.Send(new GetUserCalendarsQuery(userId)));
        }

        [Authorize]
        [HttpGet("/api/calendar/details/{id}")]
        public async Task<IActionResult> GetCalendarDetails(int id)
        {
            var query = new GetCalendarDetailQuery(new IdRequest(id));

            return Ok(await Mediator.Send(query));
        }
        #endregion

        #region Admin
        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/calendar/create")]
        public async Task<IActionResult> AdminCreateCalendar([FromBody]CreateCalendarRequest calendar)
        {
            return Ok(await Mediator.Send(new CreateCalendarCommand(calendar)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/calendars")]
        public async Task<IActionResult> AdminGetAllCalendars()
        {
            return Ok(await Mediator.Send(new GetCalendarsQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/user/calendars")]
        public async Task<IActionResult> AdminGetUserCalendars([FromBody]IdRequest user)
        {
            return Ok(await Mediator.Send(new GetUserCalendarsQuery(user)));
        }
        #endregion
    }
}