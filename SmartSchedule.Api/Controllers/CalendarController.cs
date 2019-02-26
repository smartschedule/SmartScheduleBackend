using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchedule.Application.Calendar.Commands.CreateCalendar;

namespace SmartSchedule.Api.Controllers
{
    public class CalendarController : BaseController
    {
        [HttpPost("/api/CreateCalendar")]
        public async Task<IActionResult> CreateCalendar([FromBody]CreateCalendarCommand calendar)
        {
            return Ok(await Mediator.Send(calendar));
        }
    }
}
