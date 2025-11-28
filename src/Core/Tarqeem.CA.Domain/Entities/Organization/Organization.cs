using System.ComponentModel.DataAnnotations;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.Organization;

public class Organization : BaseEntity<int>
{
    public Organization()
    {
        CreatedAt = DateTime.Now;
    }

    [MaxLength(500)] [Required] public string OrganizationName { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<User.User> Users { get; set; }
    public ICollection<Room> Rooms { get; set; }
    public ICollection<Student> Students { get; set; }
}