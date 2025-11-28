namespace Tarqeem.CA.Application.Features.Permission.Queries.GetUserPermissions;

public record GetUserPermissionsQueryResponse(
    IEnumerable<Permission> Permissions,
    string Username,
    int UserId,
    string Name
) { }

public record Permission(string Name);
