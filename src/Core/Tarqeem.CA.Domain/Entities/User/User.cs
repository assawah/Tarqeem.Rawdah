using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Tarqeem.CA.Domain.Common;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Domain.Entities.User;

public class User : IdentityUser<int>, IEntity, ISoftDeletable
{
    [Required] [MaxLength(500)] public string Name { get; set; }
    [Required] [MaxLength(500)] public string FamilyName { get; set; }
    [Required] public int Age { get; set; }
    [MaxLength(500)] public string Specialization { get; set; }
    [MaxLength(500)] public string Qualification { get; set; }

    public Organization.Organization Organization { get; set; }
    public int OrganizationId { get; set; }

    public bool IsDeleted { get; set; }


    public ICollection<Room> Rooms { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<UserLogin> Logins { get; set; }
    public ICollection<UserClaim> Claims { get; set; }
    public ICollection<UserToken> Tokens { get; set; }
    public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
}
