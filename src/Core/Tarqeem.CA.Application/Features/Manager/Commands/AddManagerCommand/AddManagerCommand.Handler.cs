using System.Security.Claims;
using AutoMapper;
using Mediator;
using Microsoft.Extensions.Logging;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.SharedKernel;
using Tarqeem.CA.SharedKernel.Extensions;

namespace Tarqeem.CA.Application.Features.Manager.Commands.AddManagerCommand
{
    internal class AddMangerCommandHandler(
        IAppUserManager userManager,
        IMapper mapper,
        ILogger<AddMangerCommandHandler> logger)
        : IRequestHandler<AddManagerCommand, OperationResult<bool>>
    {
        public async ValueTask<OperationResult<bool>> Handle(AddManagerCommand request,
            CancellationToken cancellationToken)
        {
            var newManager = mapper.Map<User>(request);
            newManager.Specialization = "manager";
            newManager.Organization = new Organization() { OrganizationName = request.OrganizationName };

            // make sure username is unique
            var user = await userManager.GetByUserName(request.UserName);
            if (user != null)
            {
                return OperationResult<bool>.FailureResult(RawdahErrors.UsernameExists);
            }

            var managerCreateResult =
                await userManager.CreateUserWithPasswordAsync(
                    newManager, request.Password);


            if (!managerCreateResult.Succeeded)
                return OperationResult<bool>.FailureResult(RawdahErrors.ServerError,
                    managerCreateResult.Errors.StringifyIdentityResultErrors(), logger);

            var managerAddPermissionResult = await userManager.AddClaimToUser(newManager,
                new Claim(nameof(DynamicPermission), nameof(DynamicPermission.IsManager)));

            if (!managerAddPermissionResult.Succeeded)
                return OperationResult<bool>.FailureResult(RawdahErrors.ServerError,
                    managerAddPermissionResult.Errors.StringifyIdentityResultErrors(), logger);

            return OperationResult<bool>.SuccessResult(true);
        }
    }
}