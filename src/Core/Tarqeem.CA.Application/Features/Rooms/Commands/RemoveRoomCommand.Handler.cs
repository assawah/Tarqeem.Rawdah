using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public class RemoveRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<RemoveRoomCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(RemoveRoomCommand request, CancellationToken cancellationToken)
    {
        // Check if room exists
        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room is null)
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        await roomRepository.RemoveRoom(request.RoomId);
        return OperationResult<bool>.SuccessResult(true);
    }
}