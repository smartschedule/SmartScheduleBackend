namespace SmartSchedule.Application.Event.Queries.GetEventList
{
    using System;
    using AutoMapper;
    using SmartSchedule.Application.Interfaces.Mapping;
    using SmartSchedule.Domain.Enums;

    public class EventLookupModel : IHaveCustomMapping
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }

        public TimeSpan? ReminderBefore { get; set; }

        public TimeSpan? RepeatsEvery { get; set; }
        public DateTime? RepeatsTo { get; set; }

        public EventTypes Type { get; set; }

        public string Name { get; set; }
        public int CalendarId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Domain.Entities.Event, EventLookupModel>()
                .ForMember(cDTO => cDTO.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(cDTO => cDTO.StartDate, opt => opt.MapFrom(c => c.StartDate))
                .ForMember(cDTO => cDTO.Duration, opt => opt.MapFrom(c => c.Duration))
                .ForMember(cDTO => cDTO.ReminderBefore, opt => opt.MapFrom(c => c.ReminderBefore))
                .ForMember(cDTO => cDTO.RepeatsEvery, opt => opt.MapFrom(c => c.RepeatsEvery))
                .ForMember(cDTO => cDTO.RepeatsTo, opt => opt.MapFrom(c => c.RepeatsTo))
                .ForMember(cDTO => cDTO.Type, opt => opt.MapFrom(c => c.Type))
                .ForMember(cDTO => cDTO.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(cDTO => cDTO.CalendarId, opt => opt.MapFrom(c => c.CalendarId))
                .ForMember(cDTO => cDTO.Longitude, opt => opt.MapFrom(c => c.Location.Longitude))
                .ForMember(cDTO => cDTO.Latitude, opt => opt.MapFrom(c => c.Location.Latitude));
        }
    }
}
