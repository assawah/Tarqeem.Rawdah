using System.ComponentModel.DataAnnotations;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.Organization;

public class Room : BaseEntity<int>
{
    public ICollection<User.User> Teachers { get; set; }
    public Organization Organization { get; set; }
    public int OrganizationId { get; set; }
    [MaxLength(500)]
    public string Name { get; set; }
    public ICollection<Student> Students { get; set; }
}