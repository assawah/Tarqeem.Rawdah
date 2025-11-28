using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Students.Queries;

public record GetStudentQuery(int StudentId)
    : IRequest<OperationResult<GetStudentQueryResponse>>, IValidatableModel<GetStudentQuery>
{
    public IValidator<GetStudentQuery> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<GetStudentQuery> validator)
    {
        validator.RuleFor(r => r.StudentId).GreaterThan(0);
        return validator;
    }
}