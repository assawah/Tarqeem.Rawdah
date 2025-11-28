using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel;

namespace Tarqeem.CA.Application.Features.Permission.Queries.GetAllPermissions;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery,
    OperationResult<List<GetAllPermissionsQueryResponse>>>
{
    public ValueTask<OperationResult<List<GetAllPermissionsQueryResponse>>> Handle(GetAllPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        List<GetAllPermissionsQueryResponse> response = [];
        response.AddRange(from DynamicPermission permission in Enum.GetValues(typeof(DynamicPermission))
            select new GetAllPermissionsQueryResponse($"{permission}", (int)permission));

        return ValueTask.FromResult(OperationResult<List<GetAllPermissionsQueryResponse>>.SuccessResult(response));
    }
}