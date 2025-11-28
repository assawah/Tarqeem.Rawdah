using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public record RegisterAllRoomExceptIdsCommand(int RoomId, IEnumerable<int> ExceptIds)
    : IRequest<OperationResult<bool>>, IValidatableModel<RegisterAllRoomExceptIdsCommand>
{
    public IValidator<RegisterAllRoomExceptIdsCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<RegisterAllRoomExceptIdsCommand> validator)
    {
        validator.RuleFor(c => c.ExceptIds).NotNull();
        validator.RuleFor(c => c.RoomId).GreaterThan(0);
        return validator;
    }
}