using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public record UpdateUserCommand
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UpdateUserCommand>,
        ICreateMapper<User>
{
    public int UserId { get; set; }

    // public string UserName { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public int Age { get; set; }
    public string Specialization { get; set; }
    public string Qualification { get; set; }

    public IValidator<UpdateUserCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateUserCommand> validator
    )
    {
        validator.RuleFor(c => c.UserId).GreaterThan(0).NotNull().NotEmpty();
        validator.RuleFor(c => c.Name).NotEmpty().NotNull();
        //validator.RuleFor(c => c.UserName).NotEmpty().NotNull();
        validator.RuleFor(c => c.FamilyName).NotEmpty().NotNull();
        validator.RuleFor(c => c.Age).GreaterThan(0).NotEqual(0);
        validator.RuleFor(c => c.Specialization).NotEmpty();

        return validator;
    }
}
