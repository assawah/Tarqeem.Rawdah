using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public record UserDeleteCommand(int UserId)
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UserDeleteCommand>
{
    public IValidator<UserDeleteCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UserDeleteCommand> validator
    )
    {
        validator.RuleFor(c => c.UserId).NotEmpty();

        return validator;
    }
}
