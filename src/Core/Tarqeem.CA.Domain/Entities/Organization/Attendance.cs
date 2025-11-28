using Tarqeem.CA.Domain.Common;

namespace Tarqeem.CA.Domain.Entities.Organization;

public class Attendance : BaseEntity<int>
{
    public Student Student { get; set; }
    public int StudentId { get; set; }
    public DateTime AttendanceDate { get; set; }
}