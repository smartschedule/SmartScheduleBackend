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
using SmartSchedule.Application.Calendar.Queries.GetCalendarList;

namespace SmartSchedule.Api.Controllers
{
    public class CalendarController : BaseController
    {
        [HttpPost("/api/CreateCalendar")]
        public async Task<IActionResult> CreateCalendar([FromBody]CreateCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [HttpPost("/api/DeleteCalendar")]
        public async Task<IActionResult> DeleteCalendar([FromBody]DeleteCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [HttpPost("/api/UpdateCalendar")]
        public async Task<IActionResult> UpdateCalendar([FromBody]UpdateCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [HttpPost("/api/AddFriendToCalendar")]
        public async Task<IActionResult> AddFriendToCalendar([FromBody]AddFriendToCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [HttpPost("/api/DeleteFriendFromCalendar")]
        public async Task<IActionResult> DeleteFriendFromCalendar([FromBody]DeleteFriendFromCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [HttpPost("/api/DeleteEventsFromCalendar")]
        public async Task<IActionResult> DeleteEventsFromCalendar([FromBody]DeleteEventsFromCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }

        [HttpGet("/api/calendars")]
        public async Task<IActionResult> GetCalendarsList()
        {
            return Ok(await Mediator.Send(new GetCalendarsListQuery()));
        }
    }
}
