using Mediator;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Permission.Queries.GetAllPermissions;

public record GetAllPermissionsQuery :IRequest<OperationResult<List<GetAllPermissionsQueryResponse>>>;