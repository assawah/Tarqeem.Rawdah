using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Manager.Commands.AddManagerCommand;

public record AddManagerCommand : IRequest<OperationResult<bool>>,
    IValidatableModel<AddManagerCommand>, ICreateMapper<User>
{
    public string UserName { get; set; }
    public string FamilyName { get; set; }
    public string Password { get; set; }
    public string OrganizationName { get; set; }
    public string Name { get; set; }

    public IValidator<AddManagerCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<AddManagerCommand> validator)
    {
        validator.RuleFor(c => c.UserName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Please specify a valid username");
        return validator;
    }
}