using Mediator;
using Microsoft.Extensions.Logging;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Models.Jwt;

namespace Tarqeem.CA.Application.Features.Users.Queries;

public class UserGetTokenQueryHandler(
    IAppUserManager userManager,
    IJwtService jwtService,
    ILogger<UserGetTokenQueryHandler> logger
) : IRequestHandler<UserGetTokenQuery, OperationResult<AccessToken>>
{
    public async ValueTask<OperationResult<AccessToken>> Handle(
        UserGetTokenQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetByUserName(request.UserName);

        if (user is null || user.IsDeleted)
            return OperationResult<AccessToken>.FailureResult(RawdahErrors.UsernameNotFound);

        var isUserLockedOut = await userManager.IsUserLockedOutAsync(user);

        if (isUserLockedOut)
            if (user.LockoutEnd != null)
                return OperationResult<AccessToken>.FailureResult(
                    RawdahErrors.LockedOut,
                    $"User is locked out. Try in {(user.LockoutEnd - DateTimeOffset.Now).Value.Minutes} Minutes",
                    logger
                );

        var passwordValidator = await userManager.AdminLogin(user, request.Password);

        if (!passwordValidator.Succeeded)
        {
            await userManager.IncrementAccessFailedCountAsync(user);
            return OperationResult<AccessToken>.FailureResult(RawdahErrors.PasswordInvalid);
        }

        var token = await jwtService.GenerateAsync(user);

        return OperationResult<AccessToken>.SuccessResult(token);
    }
}
