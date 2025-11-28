using System.Security.Claims;
using AutoMapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Features.Users.Queries;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.SharedKernel;

namespace Tarqeem.CA.Application.Features.Users.Commands;

internal class UserCreateCommandHandler(
    IAppUserManager userRepository,
    ILogger<UserGetTokenQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<UserCreateCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UserCreateCommand request,
        CancellationToken cancellationToken
    )
    {
        var userNameExist = await userRepository.IsExistUser(request.UserName);

        if (userNameExist)
            return OperationResult<bool>.FailureResult(RawdahErrors.UsernameExists);

        var user = mapper.Map<User>(request);
        // get organization ID of the creator
        var creator = await userRepository.GetUserByIdAsync(request.CreatorId);
        if (creator == null)
        {
            return OperationResult<bool>.FailureResult(
                RawdahErrors.OrganizationNotFound,
                "Organization not found",
                logger
            );
        }

        user.OrganizationId = creator.OrganizationId;

        var createResult = await userRepository.CreateUserWithPasswordAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return OperationResult<bool>.FailureResult(
                RawdahErrors.ServerError,
                string.Join(",", createResult.Errors.Select(c => c.Description)),
                logger
            );
        }

        List<Claim> newClaims = [];
        ConstantPolicies
            .TeacherPermissions.ToList()
            .ForEach(x => newClaims.Add(new Claim(nameof(DynamicPermission), x.ToString())));

        var result = await userRepository.SetUserClaims(user, newClaims);
        if (!result.Succeeded)
        {
            return OperationResult<bool>.FailureResult(
                RawdahErrors.ServerError,
                string.Join(",", createResult.Errors.Select(c => c.Description)),
                logger
            );
        }

        return OperationResult<bool>.SuccessResult(true);
    }
}
