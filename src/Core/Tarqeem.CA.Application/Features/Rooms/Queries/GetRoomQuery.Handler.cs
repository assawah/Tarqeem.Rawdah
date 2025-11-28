using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Features.Students.Queries;
using Tarqeem.CA.Application.Features.Users.Queries;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Rooms.Queries;

public class GetRoomQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    : IRequestHandler<GetRoomQuery, OperationResult<GetRoomQueryResponse>>
{
    public async ValueTask<OperationResult<GetRoomQueryResponse>> Handle(
        GetRoomQuery request,
        CancellationToken cancellationToken
    )
    {
        // make sure room exists
        var room = await roomRepository.GetRoomById(request.RoomId);
        if (room == null)
            return OperationResult<GetRoomQueryResponse>.FailureResult(RawdahErrors.RoomNotFound);

        var res = await roomRepository.GetRoomWithStudentsAndTeachers(room.Id);
        var students = mapper.Map<List<GetStudentQueryMapped>>(res.Students);
        var teachers = mapper.Map<List<GetUserDetailsQueryResponse>>(res.Teachers);
        return OperationResult<GetRoomQueryResponse>.SuccessResult(
            new GetRoomQueryResponse(res.Name, room.Id, students, teachers)
        );
    }
}
