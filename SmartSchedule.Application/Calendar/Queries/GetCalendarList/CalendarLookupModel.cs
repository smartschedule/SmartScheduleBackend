namespace SmartSchedule.Application.Calendar.Queries.GetCalendarList
{
    using AutoMapper;
    using SmartSchedule.Application.Interfaces.Mapping;
    using System.Linq;

    public class CalendarLookupModel : IHaveCustomMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }
        public int UserId { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Domain.Entities.Calendar, CalendarLookupModel>()
                .ForMember(cDTO => cDTO.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(cDTO => cDTO.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(cDTO => cDTO.ColorHex, opt=> opt.MapFrom(c=>c.ColorHex))
                .ForMember(cDTO => cDTO.UserId, opt=> opt.MapFrom(c=>c.UsersCalendars.FirstOrDefault(x=>x.CalendarId.Equals(c.Id)).UserId));
        }
    }
}
