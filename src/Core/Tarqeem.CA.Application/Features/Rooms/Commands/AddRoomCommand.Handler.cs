using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public class AddRoomCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IRoomRepository roomRepository,
    IOrganizationRepository organizationRepository)
    : IRequestHandler<AddRoomCommand, OperationResult<AddRoomCommandResponse>>
{
    public async ValueTask<OperationResult<AddRoomCommandResponse>> Handle(AddRoomCommand request,
        CancellationToken cancellationToken)
    {
        var room = mapper.Map<Room>(request);
        if (await organizationRepository.GetOrganizationById(request.OrganizationId) == null)
        {
            return OperationResult<AddRoomCommandResponse>.FailureResult(RawdahErrors.OrganizationNotFound);
        }

        var r = roomRepository.AddRoom(room).Id;
        await unitOfWork.CommitAsync();
        var res = new AddRoomCommandResponse(r) { RoomId = r };
        return OperationResult<AddRoomCommandResponse>.SuccessResult(res);
    }
}