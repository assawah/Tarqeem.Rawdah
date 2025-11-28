using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public record AddUserToRoomCommand(int TeacherId, int RoomId)
    : IRequest<OperationResult<bool>>, IValidatableModel<AddUserToRoomCommand>
{
    public IValidator<AddUserToRoomCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<AddUserToRoomCommand> validator)
    {
        validator.RuleFor(c => c.TeacherId).GreaterThan(0);
        validator.RuleFor(c => c.RoomId).GreaterThan(0);
        return validator;
    }
}