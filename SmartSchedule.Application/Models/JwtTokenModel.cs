using System;

namespace SmartSchedule.Application.Models
{
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public DateTime Duration { get; set; }
    }
}
