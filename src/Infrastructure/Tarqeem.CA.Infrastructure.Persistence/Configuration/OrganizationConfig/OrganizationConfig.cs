using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Infrastructure.Persistence.Configuration.OrganizationConfig;

internal class OrganizationConfig : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasMany(o => o.Users).WithOne(u => u.Organization).HasForeignKey(u => u.OrganizationId);
        builder.HasMany(o => o.Rooms).WithOne(o => o.Organization).HasForeignKey(o => o.OrganizationId);
        builder.HasMany(o => o.Students).WithOne(o => o.Organization).HasForeignKey(o => o.OrganizationId);
        builder.HasQueryFilter(s => s.IsDeleted == false);
    }
}