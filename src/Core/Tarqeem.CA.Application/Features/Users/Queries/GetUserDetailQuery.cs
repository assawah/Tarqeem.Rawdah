using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Queries;

public record GetUserDetailQuery(int UserId)
    : IValidatableModel<GetUserDetailQuery>, IRequest<OperationResult<GetUserDetailsQueryResponse>>
{
    public IValidator<GetUserDetailQuery> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<GetUserDetailQuery> validator)
    {
        validator.RuleFor(q => q.UserId).GreaterThan(0).NotEmpty().NotNull();
        return validator;
    }
}