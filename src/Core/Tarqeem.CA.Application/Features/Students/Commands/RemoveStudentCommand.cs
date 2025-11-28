using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public record RemoveStudentCommand(int StudentId)
    : IRequest<OperationResult<bool>>, IValidatableModel<RemoveStudentCommand>
{
    public IValidator<RemoveStudentCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<RemoveStudentCommand> validator)
    {
        validator.RuleFor(x => x.StudentId).GreaterThan(0);
        return validator;
    }
}