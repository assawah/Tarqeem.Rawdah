using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Rooms.Queries;

public class GetOrganizationRoomQueryHandler(
    IOrganizationRepository organizationRepository,
    IAppUserManager teacherRepo,
    IRoomRepository roomRepo
) : IRequestHandler<GetOrganizationRoomsQuery, OperationResult<GetOrganizationRoomsQueryResponse>>
{
    public async ValueTask<OperationResult<GetOrganizationRoomsQueryResponse>> Handle(
        GetOrganizationRoomsQuery request,
        CancellationToken cancellationToken
    )
    {
        var org = await organizationRepository.GetOrganizationByIdIncludeRoomsIncludeTeachers(
            request.OrganizationId
        );
        if (org == null)
        {
            return OperationResult<GetOrganizationRoomsQueryResponse>.FailureResult(
                RawdahErrors.OrganizationNotFound
            );
        }

        if (request.TeacherId == -1)
        {
            var rooms = org
                .Rooms.Select(room =>
                {
                    var troom = roomRepo.GetRoomWithStudentsAndTeachers(room.Id).Result;
                    var teacherCount = 0;
                    var studentCount = 0;
                    if (troom.Students != null)
                    {
                        studentCount = troom.Students.Count;
                    }

                    if (troom.Teachers != null)
                    {
                        teacherCount = troom.Teachers.Count;
                    }

                    return new SingleRoom(troom.Name, studentCount, teacherCount, troom.Id);
                })
                .ToList();

            return OperationResult<GetOrganizationRoomsQueryResponse>.SuccessResult(
                new GetOrganizationRoomsQueryResponse(rooms)
            );
        }
        else
        {
            var t = await teacherRepo.GetUserByIdAsync(request.TeacherId);
            if (t == null)
            {
                return OperationResult<GetOrganizationRoomsQueryResponse>.FailureResult(
                    RawdahErrors.UserNotFound
                );
            }

            var rs = await teacherRepo.GetUserRooms(request.TeacherId);

            var rooms = rs.Select(room =>
                {
                    var troom = roomRepo.GetRoomWithStudentsAndTeachers(room.Id).Result;
                    var teacherCount = 0;
                    var studentCount = 0;
                    if (troom.Students != null)
                    {
                        studentCount = troom.Students.Count;
                    }

                    if (troom.Teachers != null)
                    {
                        teacherCount = troom.Teachers.Count;
                    }

                    return new SingleRoom(troom.Name, studentCount, teacherCount, troom.Id);
                })
                .ToList();

            return OperationResult<GetOrganizationRoomsQueryResponse>.SuccessResult(
                new GetOrganizationRoomsQueryResponse(rooms)
            );
        }
    }
}
