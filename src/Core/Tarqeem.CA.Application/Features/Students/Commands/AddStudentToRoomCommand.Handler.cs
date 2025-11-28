using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public record AddStudentToRoomCommandHandler(
    IRoomRepository Rooms,
    IStudentRepository Students,
    IUnitOfWork Unit
) : IRequestHandler<AddStudentToRoomCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        AddStudentToRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        // make sure student exists
        var student = await Students.GetStudentByIdAsync(request.StudentId);
        if (student == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        var room = await Rooms.GetRoomById(request.RoomId);
        if (room == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        }

        if (room.Students.Any(s => s.Id == request.StudentId))
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.AlreadyAdded);
        }

        room.Students.Add(student);
        await Unit.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}
