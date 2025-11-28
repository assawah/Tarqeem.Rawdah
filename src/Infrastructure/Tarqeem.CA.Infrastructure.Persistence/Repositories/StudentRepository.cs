using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.Infrastructure.Persistence.Repositories.Common;

namespace Tarqeem.CA.Infrastructure.Persistence.Repositories;

internal class StudentRepository(ApplicationDbContext dbContext)
    : BaseAsyncRepository<Student>(dbContext),
        IStudentRepository
{
    public async Task<EntityEntry<Student>> AddStudent(Student student)
    {
        return await Entities.AddAsync(student);
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await Entities.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Student> GetStudentByIdWithRooms(int id)
    {
        return await Entities.Include(o => o.Room).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task Remove(int id)
    {
        await base.UpdateAsync(
            s => s.Id == id,
            s => s.SetProperty(student => student.IsDeleted, true)
        );
    }
}
