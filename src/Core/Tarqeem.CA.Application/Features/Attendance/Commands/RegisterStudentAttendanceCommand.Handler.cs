using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public class RegisterStudentAttendanceCommandHandler(
    IStudentRepository studentRepository,
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<RegisterStudentAttendanceCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        RegisterStudentAttendanceCommand request,
        CancellationToken cancellationToken
    )
    {
        // make sure student exist
        var student = await studentRepository.GetStudentByIdAsync(request.StudentId);
        if (student == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        // make sure attendance does not exist already
        if (
            attendanceRepository.HasAttendanceOfDay(
                request.StudentId,
                DateOnly.FromDateTime(request.Date)
            )
        )
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.AlreadyAdded);
        }

        // register it
        await attendanceRepository.RegisterAttendanceOfDay(request.StudentId, request.Date);
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}
