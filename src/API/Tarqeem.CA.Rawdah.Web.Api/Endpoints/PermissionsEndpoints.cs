using System.Security.Claims;
using Carter;
using Mediator;
using Tarqeem.CA.Application.Features.Permission.Commands.SetUserPermissions;
using Tarqeem.CA.Application.Features.Permission.Queries.GetAllPermissions;
using Tarqeem.CA.Application.Features.Permission.Queries.GetUserPermissions;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel;
using Tarqeem.CA.SharedKernel.Extensions;
using Tarqeem.CA.WebFramework.WebExtensions;

namespace Tarqeem.CA.Rawdah.Web.Api.Endpoints;

public class PermissionsEndpoints : ICarterModule
{
    private readonly string _routePrefix = "/api/v{version:apiVersion}/Permissions/";
    private readonly double _version = 1.1;
    private readonly string _tag = "Permissions";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}",
                        async (ISender sender) =>
                        {
                            var result = await sender.Send(new GetAllPermissionsQuery());
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "GetAllPermissions",
                _tag
            )
            .Produces<OperationResult<bool>>(); // wrong

        //Get current permissions
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}Current",
                        async (ClaimsPrincipal user, ISender sender) =>
                        {
                            var result = await sender.Send(
                                new GetUserPermissionQuery(int.Parse(user.Identity.GetUserId()))
                            );
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "GetCurrentUserPermissions",
                _tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<GetUserPermissionsQueryResponse>>();

        // Get permissions per ID
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{_routePrefix}User",
                        async (int id, ISender sender) =>
                        {
                            var result = await sender.Send(new GetUserPermissionQuery(id));
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "GetUserPermissionsById",
                _tag
            )
            .RequireAuthorization(c =>
                c.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    nameof(DynamicPermission.IsManager)
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<GetUserPermissionsQueryResponse>>();

        // Set user permissions
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{_routePrefix}User",
                        async (SetUserPermissionsCommand model, ISender sender) =>
                        {
                            var result = await sender.Send(model);
                            return result.ToEndpointResult();
                        }
                    ),
                _version,
                "SetUserPermissionsById",
                _tag
            )
            .RequireAuthorization(c =>
                c.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    nameof(DynamicPermission.IsManager)
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<bool>>();
    }
}
