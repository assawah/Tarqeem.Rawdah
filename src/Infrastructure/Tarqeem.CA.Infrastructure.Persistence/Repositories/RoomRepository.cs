using Microsoft.EntityFrameworkCore;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.Infrastructure.Persistence.Repositories.Common;

namespace Tarqeem.CA.Infrastructure.Persistence.Repositories;

internal class RoomRepository(ApplicationDbContext context) : BaseAsyncRepository<Room>(context), IRoomRepository
{
    public async Task AddRoom(Room room) => await base.AddAsync(room);

    public async Task<Room> GetRoomById(int id)
    {
        return await Entities.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task RemoveRoom(int id)
    {
        await base.UpdateAsync(r => r.Id == id, r => r.SetProperty(room => room.IsDeleted, true));
    }

    public async Task<Room> GetRoomWithStudentsAndTeachers(int id)
    {
        return await Entities.Where(r => r.Id == id).Include(r => r.Students).Include(r => r.Teachers)
            .FirstOrDefaultAsync();
    }
}