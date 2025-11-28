using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Rooms.Queries;

public record GetOrganizationRoomsQuery(int OrganizationId, int TeacherId)
    : IRequest<OperationResult<GetOrganizationRoomsQueryResponse>>,
        IValidatableModel<GetOrganizationRoomsQuery>
{
    public IValidator<GetOrganizationRoomsQuery> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<GetOrganizationRoomsQuery> validator
    )
    {
        validator.RuleFor(o => o.OrganizationId).GreaterThan(0).NotNull();
        return validator;
    }
}

public record GetRoomsByStudentQuery(int StudentId)
    : IRequest<OperationResult<GetOrganizationRoomsQueryResponse>>,
        IValidatableModel<GetRoomsByStudentQuery>
{
    public IValidator<GetRoomsByStudentQuery> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<GetRoomsByStudentQuery> validator
    )
    {
        validator.RuleFor(o => o.StudentId).GreaterThan(0).NotNull();
        return validator;
    }
}

public class GetRoomsByStudentQueryHandler(IStudentRepository studentRepo, IRoomRepository roomRepo)
    : IRequestHandler<GetRoomsByStudentQuery, OperationResult<GetOrganizationRoomsQueryResponse>>
{
    public async ValueTask<OperationResult<GetOrganizationRoomsQueryResponse>> Handle(
        GetRoomsByStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        var s = await studentRepo.GetStudentByIdWithRooms(request.StudentId);
        if (s == null)
        {
            return OperationResult<GetOrganizationRoomsQueryResponse>.FailureResult(
                RawdahErrors.StudentNotFound
            );
        }

        var rooms = s
            .Room.Select(room =>
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
