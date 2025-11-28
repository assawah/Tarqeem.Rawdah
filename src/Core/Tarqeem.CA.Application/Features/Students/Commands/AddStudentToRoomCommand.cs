using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public record AddStudentToRoomCommand(int RoomId, int StudentId) : IRequest<OperationResult<bool>>, IValidatableModel<AddStudentToRoomCommand>
{
    public IValidator<AddStudentToRoomCommand> ValidateApplicationModel(ApplicationBaseValidationModelProvider<AddStudentToRoomCommand> validator)
    {
        validator.RuleFor(x => x.RoomId).NotNull().NotEmpty();
        validator.RuleFor(x => x.StudentId).NotNull().NotEmpty();
        return validator;
    }
}