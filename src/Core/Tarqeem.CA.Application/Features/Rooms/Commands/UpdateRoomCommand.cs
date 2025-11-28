using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public record UpdateRoomCommand(string Name, int RoomId)
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UpdateRoomCommand>
{
    public IValidator<UpdateRoomCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateRoomCommand> validator
    )
    {
        validator.RuleFor(x => x.Name).NotEmpty();
        return validator;
    }
}
