namespace SmartSchedule.Application.DTO.Interfaces.Mapping
{
    using AutoMapper;

    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
