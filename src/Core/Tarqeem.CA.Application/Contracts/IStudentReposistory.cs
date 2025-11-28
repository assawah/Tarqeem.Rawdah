using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Contracts;

public interface IStudentRepository
{
    public Task<EntityEntry<Student>> AddStudent(Student student);
    public Task<Student> GetStudentByIdAsync(int id);
    public Task<Student> GetStudentByIdWithRooms(int id);
    public Task Remove(int id);
}
