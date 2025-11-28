using Microsoft.AspNetCore.Identity;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.User;

public class Role : IdentityRole<int>, IEntity
{
    public Role()
    {
        CreatedDate = DateTime.Now;
    }
    public DateTime CreatedDate { get; set; }
    public ICollection<RoleClaim> Claims { get; set; }
    public ICollection<UserRole> Users { get; set; }
}