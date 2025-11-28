using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Permission.Commands.SetUserPermissions;

public record SetUserPermissionsCommand(int UserId, List<int> PermissionIds)
    : IRequest<OperationResult<bool>>, IValidatableModel<SetUserPermissionsCommand>
{
    public IValidator<SetUserPermissionsCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<SetUserPermissionsCommand> validator)
    {
        validator.RuleFor(i => i.UserId).NotEmpty().WithMessage("Id cannot be empty");
        return validator;
    }
}