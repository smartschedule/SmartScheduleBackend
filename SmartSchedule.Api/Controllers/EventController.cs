﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchedule.Application.Event.Commands.CreateEvent;

namespace SmartSchedule.Api.Controllers
{
    public class EventController : BaseController
    {
        [HttpPost("/api/CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody]CreateEventCommand eventCommand)
        {
            return Ok(await Mediator.Send(eventCommand));
        }

    }
}
