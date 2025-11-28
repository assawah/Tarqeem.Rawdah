using Mediator;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Permission.Queries.GetUserPermissions;

public class GetUserPermissionsQueryHandler(IAppUserManager userManager)
    : IRequestHandler<GetUserPermissionQuery, OperationResult<GetUserPermissionsQueryResponse>>
{
    public async ValueTask<OperationResult<GetUserPermissionsQueryResponse>> Handle(
        GetUserPermissionQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            return OperationResult<GetUserPermissionsQueryResponse>.FailureResult(
                RawdahErrors.UserNotFound
            );
        }

        var claims = await userManager.GetClaimsByUser(user);
        var response = new GetUserPermissionsQueryResponse(
            claims?.Select(s => new Permission(s.Value)).ToList(),
            user.UserName,
            user.Id,
            user.Name + " " + user.FamilyName
        );
        return OperationResult<GetUserPermissionsQueryResponse>.SuccessResult(response);
    }
}
