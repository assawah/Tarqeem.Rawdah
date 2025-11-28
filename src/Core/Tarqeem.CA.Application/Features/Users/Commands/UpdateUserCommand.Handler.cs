using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public class UpdateUserCommandHandler(IAppUserManager userRepository, IMapper mapper)
    : IRequestHandler<UpdateUserCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetUserByIdAsync(request.UserId);

        if (existingUser == null)
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);

        mapper.Map(request, existingUser);
        await userRepository.UpdateUserAsync(existingUser);
        return OperationResult<bool>.SuccessResult(true); 
    }
}