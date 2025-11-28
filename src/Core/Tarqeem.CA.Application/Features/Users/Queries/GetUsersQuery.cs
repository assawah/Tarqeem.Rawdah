using System.Text.Json.Serialization;
using Mediator;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Users.Queries;

public record GetUsersQuery(
    [property: JsonIgnore] int UserId // to get the Org
) : IRequest<OperationResult<IEnumerable<GetUsersQueryResponse>>>;