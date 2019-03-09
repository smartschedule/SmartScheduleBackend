namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using AutoMapper;
    using SmartSchedule.Application.Interfaces.Mapping;
    using System;
    using System.Linq;

    public class EventLookupModel : IHaveCustomMapping
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public int CalendarId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Domain.Entities.Event, EventLookupModel>()
                .ForMember(cDTO => cDTO.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(cDTO => cDTO.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(cDTO => cDTO.StartDate, opt => opt.MapFrom(c => c.StartDate))
                .ForMember(cDTO => cDTO.EndTime, opt => opt.MapFrom(c => c.EndTime))
                .ForMember(cDTO => cDTO.CalendarId, opt => opt.MapFrom(c => c.CalendarId))
                .ForMember(cDTO => cDTO.Longitude, opt => opt.MapFrom(c => c.Location.Longitude))
                .ForMember(cDTO => cDTO.Latitude, opt => opt.MapFrom(c => c.Location.Latitude));
        }
    }
}
