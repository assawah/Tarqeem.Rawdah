using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Attendance.Commands;

public class
    RegisterAllRoomExceptIdsCommandHandler(
        IAttendanceRepository attendanceRepository,
        IRoomRepository roomRepository)
    : IRequestHandler<RegisterAllRoomExceptIdsCommand, OperationResult<bool>>

{
    public async ValueTask<OperationResult<bool>> Handle(RegisterAllRoomExceptIdsCommand request,
        CancellationToken cancellationToken)
    {
        // room exists?
        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        }

        // get all room 

        var students = attendanceRepository.GetRegistrableStudent(room.Students);
        students = students.Where(s => !request.ExceptIds.Contains(s.Id));
        foreach (var student in students)
        {
            await attendanceRepository.RegisterAttendanceOfDay(student.Id, DateTime.Now);
        }

        return OperationResult<bool>.SuccessResult(true);
    }
}