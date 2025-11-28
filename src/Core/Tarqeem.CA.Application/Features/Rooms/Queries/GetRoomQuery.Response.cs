using Tarqeem.CA.Application.Features.Students.Queries;
using Tarqeem.CA.Application.Features.Users.Queries;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Features.Rooms.Queries;

public record GetRoomQueryResponse(
    string Name,
    int Id,
    IEnumerable<GetStudentQueryMapped> Students,
    IEnumerable<GetUserDetailsQueryResponse> Teachers
) : ICreateMapper<Room>;
