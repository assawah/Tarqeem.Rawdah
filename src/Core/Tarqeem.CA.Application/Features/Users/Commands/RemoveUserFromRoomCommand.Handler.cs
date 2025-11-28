using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Users.Commands;

public class RemoveUserFromRoomCommandHandler(
    IAppUserManager teacherRepository,
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveUserFromRoomCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(RemoveUserFromRoomCommand request,
        CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetUserByIdAsync(request.TeacherId);
        if (teacher == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        }

        if (room.Teachers.All(x => x.Id != request.TeacherId))
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.NotIn);
        }

        room.Teachers.Remove(teacher);
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}