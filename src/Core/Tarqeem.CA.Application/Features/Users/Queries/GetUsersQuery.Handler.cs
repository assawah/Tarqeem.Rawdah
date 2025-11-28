using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Users.Queries;

public class GetUsersQueryHandler(
    IAppUserManager userManager,
    IOrganizationRepository organizationRepository
) : IRequestHandler<GetUsersQuery, OperationResult<IEnumerable<GetUsersQueryResponse>>>
{
    public async ValueTask<OperationResult<IEnumerable<GetUsersQueryResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var u = await userManager.GetUserByIdAsync(request.UserId);
        var org = await organizationRepository.GetOrganizationByIdIncludeRoomsIncludeTeachers(
            u.OrganizationId
        );
        if (org == null)
            return OperationResult<IEnumerable<GetUsersQueryResponse>>.FailureResult(
                RawdahErrors.OrganizationNotFound
            );

        var teachers = org
            .Users.Select(s =>
            {
                var roomCount = s.Rooms?.Count ?? 0;
                return new GetUsersQueryResponse(
                    s.Id,
                    s.UserName,
                    s.Name + " " + s.FamilyName,
                    s.Specialization,
                    roomCount,
                    s.IsDeleted
                );
            })
            .OrderBy(s => s.IsDeleted);

        return OperationResult<IEnumerable<GetUsersQueryResponse>>.SuccessResult(teachers);
    }
}
