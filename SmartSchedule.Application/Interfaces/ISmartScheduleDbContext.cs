
namespace SmartSchedule.Application.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Domain.Entities;

    public interface ISmartScheduleDbContext
    {
        DbSet<Calendar> Calendars { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<Friends> Friends { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<UserCalendar> UserCalendars { get; set; }
        DbSet<UserEvent> UserEvents { get; set; }
        DbSet<User> Users { get; set; }
    }
}
