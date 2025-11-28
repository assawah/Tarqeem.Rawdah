using Microsoft.AspNetCore.Identity;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.User;

public class UserClaim:IdentityUserClaim<int>,IEntity
{
    public User User { get; set; }
}