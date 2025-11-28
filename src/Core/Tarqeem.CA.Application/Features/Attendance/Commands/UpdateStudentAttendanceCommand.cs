using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public record UpdateStudentAttendanceCommand
    : IRequest<OperationResult<bool>>, IValidatableModel<UpdateStudentAttendanceCommand>
{
    public IValidator<UpdateStudentAttendanceCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateStudentAttendanceCommand> validator)
    {
        validator.RuleFor(c => c.StudentId).NotNull().GreaterThan(0);
        validator.RuleFor(c => c.OldDate).NotNull();
        validator.RuleFor(c => c.NewDate).NotNull();
        return validator;
    }

    public int StudentId { get; set; }
    public DateTime OldDate { get; set; }
    public DateTime NewDate { get; set; }
}