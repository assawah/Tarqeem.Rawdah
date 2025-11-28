using System.Security.Claims;
using Carter;
using Mediator;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Features.Rooms.Commands;
using Tarqeem.CA.Application.Features.Rooms.Queries;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel;
using Tarqeem.CA.SharedKernel.Extensions;
using Tarqeem.CA.WebFramework.WebExtensions;

namespace Tarqeem.CA.Rawdah.Web.Api.Endpoints;

// ReSharper disable once UnusedType.Global
public class RoomsEndpoints : ICarterModule
{
    private const string RoutePrefix = "/api/v{version:apiVersion}/Room/";
    private const double Version = 1.1;
    private const string Tag = "Room";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // get a room
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{RoutePrefix}",
                        async (ISender sender, int roomId, IAppUserManager appUserManager) =>
                        {
                            var res = await sender.Send(
                                new GetRoomQuery() with
                                {
                                    RoomId = roomId,
                                }
                            );
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "GetRoom",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<GetRoomQueryResponse>>();

        // update room students
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{RoutePrefix}Students",
                        async (
                            ISender sender,
                            IAppUserManager appUserManager,
                            UpdateRoomStudentsCommand model
                        ) =>
                        {
                            var res = await sender.Send(model);
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "UpdateRoomStudennts",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageRooms.ToString()
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<GetOrganizationRoomsQueryResponse>>();

        // update room teachers
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{RoutePrefix}Users",
                        async (
                            ISender sender,
                            IAppUserManager appUserManager,
                            UpdateRoomUsersCommand model
                        ) =>
                        {
                            var res = await sender.Send(model);
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "UpdateRoomUsers",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageRooms.ToString()
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<bool>>();

        // get all rooms by student
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{RoutePrefix}Student",
                        async (
                            int studentId,
                            ISender sender,
                            IAppUserManager appUserManager,
                            ClaimsPrincipal claimsPrincipal
                        ) =>
                        {
                            var res = await sender.Send(new GetRoomsByStudentQuery(studentId));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "GetStudentRooms",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<GetOrganizationRoomsQueryResponse>>();

        // get all rooms
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{RoutePrefix}All",
                        async (
                            ISender sender,
                            IAppUserManager appUserManager,
                            ClaimsPrincipal claimsPrincipal
                        ) =>
                        {
                            var id = int.Parse(claimsPrincipal.Identity.GetUserId());
                            var org = appUserManager.GetUserByIdAsync(id).Result.OrganizationId;
                            var res = await sender.Send(new GetOrganizationRoomsQuery(org, -1));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "GetAllRooms",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<GetOrganizationRoomsQueryResponse>>();

        // get all rooms by teacher
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{RoutePrefix}User",
                        async (
                            int userId,
                            ISender sender,
                            IAppUserManager appUserManager,
                            ClaimsPrincipal claimsPrincipal
                        ) =>
                        {
                            var id = int.Parse(claimsPrincipal.Identity.GetUserId());
                            var org = appUserManager.GetUserByIdAsync(id).Result.OrganizationId;
                            var res = await sender.Send(new GetOrganizationRoomsQuery(org, userId));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "GetAllRoomsByUserId",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<GetOrganizationRoomsQueryResponse>>();

        // RawdahErrors.UserNotFound
        // add room command
        app.MapEndpoint(
                builder =>
                    builder.MapPost(
                        $"{RoutePrefix}",
                        async (
                            AddRoomCommand model,
                            ISender sender,
                            IAppUserManager appUserManager,
                            ClaimsPrincipal claimsPrincipal
                        ) =>
                        {
                            var id = int.Parse(claimsPrincipal.Identity.GetUserId());
                            var res = await sender.Send(
                                model with
                                {
                                    OrganizationId = appUserManager
                                        .GetUserByIdAsync(id)
                                        .Result.OrganizationId,
                                }
                            );
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "AddRoom",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageRooms.ToString()
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<AddRoomCommandResponse>>();

        //update room command
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{RoutePrefix}",
                        async (UpdateRoomCommand model, ISender sender) =>
                        {
                            var res = await sender.Send(model);
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "UpdateRoom",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageRooms.ToString()
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.RoomNotFound))
            .Produces<OperationResult<bool>>();

        //remove room
        app.MapEndpoint(
                builder =>
                    builder.MapDelete(
                        $"{RoutePrefix}",
                        async (int roomId, ISender sender) =>
                        {
                            var res = await sender.Send(new RemoveRoomCommand(roomId));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "RemoveRoom",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageRooms.ToString()
                )
            )
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.RoomNotFound))
            .Produces<OperationResult<bool>>();
    }
}
