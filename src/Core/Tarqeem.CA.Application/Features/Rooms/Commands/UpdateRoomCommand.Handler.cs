using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public class UpdateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRoomCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        //check if room exists
        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room is null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        }

        room.Name = request.Name;
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}