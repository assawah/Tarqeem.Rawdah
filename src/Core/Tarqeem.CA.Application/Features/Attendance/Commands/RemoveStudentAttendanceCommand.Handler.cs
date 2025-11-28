using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public class RemoveStudentAttendanceCommandHandler(
    IStudentRepository studentRepository,
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RemoveStudentAttendanceCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        RemoveStudentAttendanceCommand request,
        CancellationToken cancellationToken
    )
    {
        var student = await studentRepository.GetStudentByIdAsync(request.StudentId);
        if (student == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        await attendanceRepository.RemoveAttendanceOfDay(student.Id, request.Date);
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}
