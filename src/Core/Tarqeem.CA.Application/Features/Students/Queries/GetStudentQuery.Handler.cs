using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Students.Queries;

public class GetStudentQueryHandler(IStudentRepository students, IMapper mapper)
    : IRequestHandler<GetStudentQuery, OperationResult<GetStudentQueryResponse>>
{
    public async ValueTask<OperationResult<GetStudentQueryResponse>> Handle(
        GetStudentQuery request,
        CancellationToken cancellationToken
    )
    {
        var student = await students.GetStudentByIdWithRooms(request.StudentId);
        if (student == null)
        {
            return OperationResult<GetStudentQueryResponse>.FailureResult(
                RawdahErrors.UserNotFound
            );
        }

        var studentResponse = mapper.Map<GetStudentQueryMapped>(student);
        var l = new List<GetStudentQueryMapped> { studentResponse };
        var res = new GetStudentQueryResponse(l);
        return OperationResult<GetStudentQueryResponse>.SuccessResult(res);
    }
}
