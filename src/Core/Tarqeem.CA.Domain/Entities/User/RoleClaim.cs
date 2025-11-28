using Microsoft.AspNetCore.Identity;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.User;

public class RoleClaim:IdentityRoleClaim<int>,IEntity
{
    public RoleClaim()
    {
        CreatedClaim=DateTime.Now;
    }

    public DateTime CreatedClaim { get; set; }
    public Role Role { get; set; }

}