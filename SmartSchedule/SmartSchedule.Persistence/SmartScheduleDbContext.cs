using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Domain.Entities;

namespace SmartSchedule.Persistence
{
    public class SmartScheduleDbContext : DbContext
    {
        public SmartScheduleDbContext(DbContextOptions<SmartScheduleDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Location> Locations { get; set; }      
        public virtual DbSet<UserCalendar> UserCalendars { get; set; }
        public virtual DbSet<UserEvents> UserEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartScheduleDbContext).Assembly);
        }
    }
}
