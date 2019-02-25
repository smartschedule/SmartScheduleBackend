using System;

namespace SmartSchedule.Application.Models
{
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
