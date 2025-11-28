using Mediator;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Permission.Queries.GetUserPermissions;

public record GetUserPermissionQuery(int UserId): IRequest<OperationResult<GetUserPermissionsQueryResponse>>;
