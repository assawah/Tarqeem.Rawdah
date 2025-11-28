using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public record RegisterStudentAttendanceCommand(int StudentId, DateTime Date)
    : IRequest<OperationResult<bool>>, IValidatableModel<RegisterStudentAttendanceCommand>
{
    public IValidator<RegisterStudentAttendanceCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<RegisterStudentAttendanceCommand> validator)
    {
        validator.RuleFor(c => c.StudentId).NotNull().GreaterThan(0);
        validator.RuleFor(c => c.Date).NotNull();
        return validator;
    }
}