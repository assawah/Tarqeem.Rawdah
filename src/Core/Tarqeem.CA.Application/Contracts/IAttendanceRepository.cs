using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Contracts;

public interface IAttendanceRepository
{
    public bool HasAttendanceOfDay(int studentId, DateOnly date);
    public Task RemoveAttendanceOfDay(int studentId, DateOnly date);
    public Task UpdateAttendanceOfDay(int studentId, DateTime oldDate, DateTime newDate);
    public Task RegisterAttendanceOfDay(int studentId, DateTime date);
    public IEnumerable<Student> GetRegistrableStudent(IEnumerable<Student> studentId);
}