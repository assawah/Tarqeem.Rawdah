using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Infrastructure.Persistence.Configuration.OrganizationConfig;

internal class AttendanceConfig : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasOne(s => s.Student).WithMany(s => s.Attendance).HasForeignKey(s => s.StudentId);
        builder.HasQueryFilter(s => s.IsDeleted == false);
    }
}