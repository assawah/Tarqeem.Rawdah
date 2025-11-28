using System.Security.Claims;
using Carter;
using Mediator;
using Tarqeem.CA.Application.Features.Users.Commands;
using Tarqeem.CA.Application.Features.Users.Queries;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Models.Jwt;
using Tarqeem.CA.SharedKernel;
using Tarqeem.CA.SharedKernel.Extensions;
using Tarqeem.CA.WebFramework.WebExtensions;

namespace Tarqeem.CA.Rawdah.Web.Api.Endpoints;

// ReSharper disable once UnusedType.Global
public class UserEndpoints : ICarterModule
{
    private readonly string _routePrefix = "/api/v{version:apiVersion}/Users/";
    private readonly double _version = 1.1;
    private readonly string _tag = "User";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Add new user
        app.MapEndpoint(
                builder =>
                    builder.MapPost(
                        $"{_routePrefix}",
                        async (UserCreateCommand model, ISender sender, ClaimsPrincipal user) =>
                        {
                            var userid = int.Parse(user.Identity.GetUserId());
                            var result = await sender.Send(model with { CreatorId = userid });
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "AddUser",
                _tag
            )
            .RequireAuthorization(c =>
                c.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    nameof(DynamicPermission.CanAddUsers),
                    nameof(DynamicPermission.IsManager)
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UsernameExists))
            .Produces<OperationResult<bool>>();

        // Edit a user
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{_routePrefix}",
                        async (UpdateUserCommand model, ISender sender) =>
                        {
                            var result = await sender.Send(model);
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "UpdateUser",
                _tag
            )
            .RequireAuthorization(c =>
                c.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    nameof(DynamicPermission.CanAddUsers),
                    nameof(DynamicPermission.IsManager)
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UsernameNotFound))
            .Produces<OperationResult<bool>>();

        // Update user password
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{_routePrefix}Password",
                        async (UpdateUserPasswordCommand model, ISender sender) =>
                        {
                            var result = await sender.Send(model);
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "UpdateUserPassword",
                _tag
            )
            .RequireAuthorization(c =>
                c.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    nameof(DynamicPermission.CanAddUsers),
                    nameof(DynamicPermission.IsManager)
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<bool>>();

        // Get a user
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}",
                        async (int userId, ISender sender) =>
                        {
                            var result = await sender.Send(new GetUserDetailQuery(userId));
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "GetUser",
                _tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<GetUserDetailsQueryResponse>>();

        // Get users
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}All",
                        async (ISender sender, ClaimsPrincipal user) =>
                        {
                            var userid = int.Parse(user.Identity.GetUserId());
                            var result = await sender.Send(new GetUsersQuery(userid));
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "GetUsers",
                _tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<IEnumerable<GetUsersQueryResponse>>>();

        // Login
        app.MapEndpoint(
                builder =>
                    builder.MapPost(
                        $"{_routePrefix}Login",
                        async (UserGetTokenQuery model, ISender sender) =>
                        {
                            var result = await sender.Send(model);
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "Login",
                _tag
            )
            .WithOpenApi(op =>
                Utl.WriteOpenApiErrors(
                    op,
                    RawdahErrors.UsernameNotFound,
                    RawdahErrors.PasswordInvalid
                )
            )
            .Produces<OperationResult<AccessToken>>();

        //delete a user
        app.MapEndpoint(
                builder =>
                    builder.MapDelete(
                        $"{_routePrefix}",
                        async (ISender sender, int userId) =>
                        {
                            var result = await sender.Send(new UserDeleteCommand(userId));
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "DeleteUser",
                _tag
            )
            .RequireAuthorization(c =>
                c.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    nameof(DynamicPermission.CanAddUsers),
                    nameof(DynamicPermission.IsManager)
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<IEnumerable<GetUsersQueryResponse>>>();

        // Logout
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}Logout",
                        async (ClaimsPrincipal user, ISender sender) =>
                        {
                            var result = await sender.Send(
                                new RequestLogoutCommand(int.Parse(user.Identity.GetUserId()))
                            );
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "Logout",
                _tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<bool>>();

        //Refresh
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}RefreshSignIn",
                        async (Guid userRefreshToken, ISender sender) =>
                        {
                            var result = await sender.Send(
                                new RefreshUserTokenCommand(userRefreshToken)
                            );
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "RefreshSignIn",
                _tag
            )
            .Produces<OperationResult<bool>>();
    }
}
