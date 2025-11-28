using System.ComponentModel.DataAnnotations;
using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.Organization;

public class Student : BaseEntity<int>
{
    public ICollection<Room> Room { get; set; }
    public ICollection<Attendance> Attendance { get; set; }
    public Organization Organization { get; set; }
    public int OrganizationId { get; set; }
    public int Age { get; set; }
    public byte[] ProfilePicture { get; set; } // Storing the profile picture as a byte array
    [MaxLength(500)] public string FullName { get; set; }
    [MaxLength(500)] public string Address { get; set; }
    [MaxLength(100)] public string PhoneNumber { get; set; }
    [MaxLength(4000)] public string Note { get; set; }
}