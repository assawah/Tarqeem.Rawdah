using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Infrastructure.Persistence.Configuration.OrganizationConfig;

internal class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasMany(s => s.Room).WithMany(r => r.Students);
        builder.HasOne(s => s.Organization).WithMany(o => o.Students).HasForeignKey(s => s.OrganizationId);
        builder.HasMany(s => s.Attendance).WithOne(a => a.Student).HasForeignKey(a => a.StudentId);
        builder.HasQueryFilter(s => s.IsDeleted == false);
    }
}