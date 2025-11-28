using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public class UpdateStudentAttendanceCommandHandler(
    IStudentRepository studentRepository,
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateStudentAttendanceCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UpdateStudentAttendanceCommand request,
        CancellationToken cancellationToken
    )
    {
        var student = await studentRepository.GetStudentByIdAsync(request.StudentId);
        if (student == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        await attendanceRepository.UpdateAttendanceOfDay(
            student.Id,
            request.OldDate,
            request.NewDate
        );
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}
