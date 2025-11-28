using Microsoft.AspNetCore.Identity;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.User;

public class UserToken:IdentityUserToken<int>,IEntity
{
    public UserToken()
    {
        GeneratedTime=DateTime.Now;
    }

    public User User { get; set; }
    public DateTime GeneratedTime { get; set; }

}