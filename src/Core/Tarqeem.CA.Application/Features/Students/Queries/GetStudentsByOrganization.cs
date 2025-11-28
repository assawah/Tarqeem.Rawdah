using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Students.Queries;

public record GetStudentsByOrganizationQuery(int OrganizationId)
    : IRequest<OperationResult<GetStudentQueryResponse>>,
        IValidatableModel<GetStudentsByOrganizationQuery>
{
    public IValidator<GetStudentsByOrganizationQuery> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<GetStudentsByOrganizationQuery> validator
    )
    {
        validator
            .RuleFor(x => x.OrganizationId)
            .NotEqual(0)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id cannot be empty");
        return validator;
    }
}
