using System.Text.Json.Serialization;
using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public record RequestLogoutCommand([property: JsonIgnore] int UserId)
    : IRequest<OperationResult<bool>>, IValidatableModel<RequestLogoutCommand>
{
    public IValidator<RequestLogoutCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<RequestLogoutCommand> validator)
    {
        validator.RuleFor(x => x.UserId).NotEmpty().NotNull().NotEmpty().WithMessage("UserId cannot be empty");
        return validator;
    }
}