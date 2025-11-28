namespace Tarqeem.CA.Application.Features.Rooms.Queries;

public record GetOrganizationRoomsQueryResponse(IEnumerable<SingleRoom> Rooms);

public record SingleRoom(string Name, int StudentCount, int TeacherCount, int RoomId);
