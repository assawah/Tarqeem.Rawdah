using System.Security.Claims;
using Mediator;
using Microsoft.Extensions.Logging;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel;
using Tarqeem.CA.SharedKernel.Extensions;

namespace Tarqeem.CA.Application.Features.Permission.Commands.SetUserPermissions;

public class SetUserPermissionsCommandHandler(
    IAppUserManager userManager,
    ILogger<SetUserPermissionsCommandHandler> logger)
    : IRequestHandler<SetUserPermissionsCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(SetUserPermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        var newClaims = new List<Claim>();


        request.PermissionIds.Where(n => Enum.IsDefined(typeof(DynamicPermission), n))
            .Select(n => (DynamicPermission)n)
            .ToList().ForEach(x => newClaims.Add(new Claim(nameof(DynamicPermission), x.ToString())));

        var result = await userManager.SetUserClaims(user, newClaims);
        return !result.Succeeded
            ? OperationResult<bool>.FailureResult(RawdahErrors.ServerError,
                result.Errors.StringifyIdentityResultErrors(), logger)
            : OperationResult<bool>.SuccessResult(true);
    }
}