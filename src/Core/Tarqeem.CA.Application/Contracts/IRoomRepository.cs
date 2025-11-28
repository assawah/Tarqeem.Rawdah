using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Contracts;

public interface IRoomRepository
{
    public Task AddRoom(Room room);
    public Task<Room> GetRoomById(int id);
    public Task RemoveRoom(int id);
    public Task<Room> GetRoomWithStudentsAndTeachers(int id);
}