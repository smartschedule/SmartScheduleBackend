namespace SmartSchedule.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SmartSchedule.Domain.Entities;

    public class FriendsConfiguration : IEntityTypeConfiguration<Friends>
    {
        public void Configure(EntityTypeBuilder<Friends> builder)
        {
            builder.Property(x => x.Type).HasConversion<int>();
            builder.HasKey("FirstUserId","SecoundUserId");
        }
    }
}
