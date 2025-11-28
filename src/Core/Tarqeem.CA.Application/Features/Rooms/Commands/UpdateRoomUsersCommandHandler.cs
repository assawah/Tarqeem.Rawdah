using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Domain.Entities.User;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public class UpdateRoomUsersCommandHandler(
    IRoomRepository roomRepo,
    IAppUserManager userRepo,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateRoomUsersCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UpdateRoomUsersCommand request,
        CancellationToken cancellationToken
    )
    {
        var r = await roomRepo.GetRoomWithStudentsAndTeachers(request.RoomId);
        if (r == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        }
        var nusers = new List<User>();
        foreach (var t in request.TeacherIds)
        {
            var nt = await userRepo.GetUserByIdAsync(t);
            if (nt == null)
            {
                return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
            }
            nusers.Add(nt);
        }
        r.Teachers = nusers;
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}
