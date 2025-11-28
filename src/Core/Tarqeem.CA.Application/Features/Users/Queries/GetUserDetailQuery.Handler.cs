using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Users.Queries;

public class
    GetUserDetailQueryHandler(IAppUserManager userManager, IMapper mapper)
    : IRequestHandler<GetUserDetailQuery, OperationResult<GetUserDetailsQueryResponse>>
{
    public async ValueTask<OperationResult<GetUserDetailsQueryResponse>> Handle(GetUserDetailQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserByIdAsync(request.UserId);
        if (user == null)
        {
            return OperationResult<GetUserDetailsQueryResponse>.FailureResult(RawdahErrors.UserNotFound);
        }

        var result = mapper.Map<GetUserDetailsQueryResponse>(user);
        return OperationResult<GetUserDetailsQueryResponse>.SuccessResult(result);
    }
}