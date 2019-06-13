namespace SmartSchedule.Application.Infrastructure.AutoMapper
{
    using global::AutoMapper;
    using SmartSchedule.Application.DTO.Interfaces.Mapping;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            LoadStandardMappings();
            LoadCustomMappings();
        }

        private void LoadStandardMappings()
        {
            var mapsFrom = MapperProfileHelper.LoadStandardMappings(typeof(IHaveCustomMapping).Assembly);

            foreach (var map in mapsFrom)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            var mapsFrom = MapperProfileHelper.LoadCustomMappings(typeof(IHaveCustomMapping).Assembly);

            foreach (var map in mapsFrom)
            {
                map.CreateMappings(this);
            }
        }
    }

}

