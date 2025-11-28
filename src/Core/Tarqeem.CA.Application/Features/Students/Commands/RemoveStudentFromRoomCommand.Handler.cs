using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public class RemoveStudentFromRoomCommandHandler(IStudentRepository students, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveStudentFromRoomCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(RemoveStudentFromRoomCommand request,
        CancellationToken cancellationToken)
    {
        // get student
        var student = await students.GetStudentByIdWithRooms(request.StudentId);
        if (students == null)
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        // is student in that room?
        if (student.Room.Any(r => r.Id == request.RoomId))
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.NotIn);
        }

        student.Room.Remove(student.Room.FirstOrDefault(r => r.Id == request.RoomId));
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}