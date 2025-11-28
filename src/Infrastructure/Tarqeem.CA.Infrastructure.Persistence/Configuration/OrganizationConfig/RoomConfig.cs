using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Infrastructure.Persistence.Configuration.OrganizationConfig;

internal class RoomConfig : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasOne(s => s.Organization).WithMany(s => s.Rooms).HasForeignKey(s => s.OrganizationId);
        builder.HasMany(r => r.Teachers).WithMany(t => t.Rooms);
        builder.HasMany(r => r.Students).WithMany(s => s.Room);
        builder.HasQueryFilter(s => s.IsDeleted == false);
    }
}