using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Features.Users.Queries;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public record UpdateUserPasswordCommand(int UserId, string Password)
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UpdateUserPasswordCommand>
{
    public IValidator<UpdateUserPasswordCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateUserPasswordCommand> validator
    )
    {
        validator.RuleFor(c => c.UserId).NotEmpty();
        validator.RuleFor(c => c.Password).NotEmpty();
        return validator;
    }
}

public class UpdateUserPasswordCommandHandler(
    IAppUserManager userRepository,
    ILogger<UserGetTokenQueryHandler> logger
) : IRequestHandler<UpdateUserPasswordCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UpdateUserPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var teacher = await userRepository.GetUserByIdAsync(request.UserId);
        if (teacher == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }
        var result = await userRepository.UpdatePasswordAsync(teacher, request.Password);

        if (!result.Succeeded)
        {
            return OperationResult<bool>.FailureResult(
                RawdahErrors.ServerError,
                string.Join(",", result.Errors.Select(c => c.Description)),
                logger
            );
        }

        return OperationResult<bool>.SuccessResult(true);
    }
}
