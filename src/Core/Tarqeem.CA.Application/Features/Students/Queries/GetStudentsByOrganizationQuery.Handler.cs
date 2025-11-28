using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Students.Queries;

public class GetStudentsByOrganizationQueryHandler(
    IOrganizationRepository organizationRepository,
    IMapper mapper
) : IRequestHandler<GetStudentsByOrganizationQuery, OperationResult<GetStudentQueryResponse>>
{
    public async ValueTask<OperationResult<GetStudentQueryResponse>> Handle(
        GetStudentsByOrganizationQuery request,
        CancellationToken cancellationToken
    )
    {
        var org = await organizationRepository.GetOrganizationByIdIncludeRoomsIncludeTeachers(
            request.OrganizationId
        );
        if (org == null)
        {
            return OperationResult<GetStudentQueryResponse>.FailureResult(
                RawdahErrors.OrganizationNotFound
            );
        }

        var studentResponse = mapper.Map<IEnumerable<GetStudentQueryMapped>>(org.Students);
        return OperationResult<GetStudentQueryResponse>.SuccessResult(
            new GetStudentQueryResponse(studentResponse)
        );
    }
}
