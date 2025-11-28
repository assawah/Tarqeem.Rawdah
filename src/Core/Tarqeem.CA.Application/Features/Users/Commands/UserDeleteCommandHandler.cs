using Mediator;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Users.Commands;

internal class UserDeleteCommandHandler(IAppUserManager userRepository)
    : IRequestHandler<UserDeleteCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UserDeleteCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId);

        if (user == null)
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        user.IsDeleted = !user.IsDeleted;
        await userRepository.UpdateUserAsync(user);

        return OperationResult<bool>.SuccessResult(true);
    }
}
