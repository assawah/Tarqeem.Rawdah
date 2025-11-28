using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Rooms.Queries;

public record GetRoomQuery : IRequest<OperationResult<GetRoomQueryResponse>>, IValidatableModel<GetRoomQuery>
{
    public int RoomId { get; set; }

    public IValidator<GetRoomQuery> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<GetRoomQuery> validator)
    {
        validator.RuleFor(r => r.RoomId).NotEqual(0);
        validator.RuleFor(r => r.RoomId).NotEmpty();
        return validator;
    }
}