using System.Text.Json.Serialization;
using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public record UserCreateCommand
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UserCreateCommand>,
        ICreateMapper<User>
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public string Specialization { get; set; }

    [JsonIgnore]
    public int CreatorId { get; set; }
    public string Qualification { get; set; }

    public IValidator<UserCreateCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UserCreateCommand> validator
    )
    {
        validator.RuleFor(c => c.Name).NotEmpty().NotNull();
        validator.RuleFor(c => c.UserName).NotEmpty().NotNull();
        validator.RuleFor(c => c.FamilyName).NotEmpty().NotNull();
        validator.RuleFor(c => c.Password).NotEmpty().NotNull().MinimumLength(4);
        validator.RuleFor(c => c.Age).GreaterThan(0).NotEqual(0);
        validator.RuleFor(c => c.Specialization).NotEmpty();

        return validator;
    }
}
