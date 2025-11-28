using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarqeem.CA.Domain.Entities.User;

namespace Tarqeem.CA.Infrastructure.Persistence.Configuration.UserConfig;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "usr").Property(p => p.Id).HasColumnName("UserId");
        // builder.HasQueryFilter(s => s.IsDeleted == false);
    }
}
