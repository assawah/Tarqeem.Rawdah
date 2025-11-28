using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public record RemoveRoomCommand(int RoomId) : IRequest<OperationResult<bool>>, IValidatableModel<RemoveRoomCommand>
{
    public IValidator<RemoveRoomCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<RemoveRoomCommand> validator)
    {
        validator.RuleFor(x => x.RoomId).NotEmpty();
        return validator;
    }
}