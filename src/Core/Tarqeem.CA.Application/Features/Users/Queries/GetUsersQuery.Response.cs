namespace Tarqeem.CA.Application.Features.Users.Queries;

public record GetUsersQueryResponse(
    int UserId,
    string Username,
    string FullName,
    string Specialization,
    int RoomsCount,
    bool IsDeleted
);
