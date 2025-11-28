using Microsoft.EntityFrameworkCore;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.Infrastructure.Persistence.Repositories.Common;

namespace Tarqeem.CA.Infrastructure.Persistence.Repositories;

internal class OrganizationRepository(ApplicationDbContext context)
    : BaseAsyncRepository<Organization>(context), IOrganizationRepository
{
    public async Task<Organization> GetOrganizationById(int organizationId) =>
        await Entities.FirstOrDefaultAsync(o => o.Id == organizationId);

    public async Task<Organization> GetOrganizationByIdIncludeRoomsIncludeTeachers(int organizationId)
    {
        return await Entities.Include(o => o.Students).Include(o => o.Users).Include(o => o.Rooms)
            .FirstOrDefaultAsync(o => o.Id == organizationId);
    }
}

internal class AttendanceRepository(ApplicationDbContext dbContext)
    : BaseAsyncRepository<Attendance>(dbContext), IAttendanceRepository
{
    public bool HasAttendanceOfDay(int studentId, DateOnly date)
    {
        return Entities.Where(o => o.StudentId == studentId)
            .Any(s => DateOnly.FromDateTime(s.AttendanceDate) == date);
    }

    public async Task RemoveAttendanceOfDay(int studentId, DateOnly date)
    {
        await base.UpdateAsync(s => s.StudentId == studentId && DateOnly.FromDateTime(s.AttendanceDate).Equals(date),
            s => s.SetProperty(a => a.IsDeleted, true));
    }

    public async Task UpdateAttendanceOfDay(int studentId, DateTime oldDate, DateTime newDate)
    {
        await base.UpdateAsync(
            s => s.StudentId == studentId && s.AttendanceDate == oldDate,
            s => s.SetProperty(a => a.AttendanceDate, newDate));
    }

    public async Task RegisterAttendanceOfDay(int studentId, DateTime date)
    {
        await Entities.AddAsync(new Attendance { StudentId = studentId, AttendanceDate = date, IsDeleted = false });
    }


    public IEnumerable<Student> GetRegistrableStudent(IEnumerable<Student> studentIds)
    {
        var all = Entities.Where(s => s.AttendanceDate.Equals(DateTime.Today)).Select(a => a.StudentId).ToList();
        var res = studentIds.Where(id => all.All(aid => id.Id != aid));
        return res;
    }
}