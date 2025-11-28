using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public record RemoveStudentFromRoomCommand(int RoomId, int StudentId)
    : IRequest<OperationResult<bool>>, IValidatableModel<RemoveStudentFromRoomCommand>
{
    public IValidator<RemoveStudentFromRoomCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<RemoveStudentFromRoomCommand> validator)
    {
        validator.RuleFor(x => x.RoomId).NotEmpty().GreaterThan(0);
        validator.RuleFor(x => x.StudentId).NotEmpty().GreaterThan(0);
        return validator;
    }
}