using System.Security.Claims;
using Carter;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Features.Students.Commands;
using Tarqeem.CA.Application.Features.Students.Queries;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.SharedKernel;
using Tarqeem.CA.SharedKernel.Extensions;
using Tarqeem.CA.WebFramework.WebExtensions;

namespace Tarqeem.CA.Rawdah.Web.Api.Endpoints;

// ReSharper disable once UnusedType.Global
public class StudentsEndpoints : ICarterModule
{
    private const string RoutePrefix = "/api/v{version:apiVersion}/Student/";
    private const double Version = 1.1;
    private const string Tag = "Student";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // add student
        app.MapEndpoint(
                builder =>
                    builder.MapPost(
                        $"{RoutePrefix}",
                        async (
                            [FromForm] AddStudentCommand model,
                            ISender sender,
                            IAppUserManager appUserManager,
                            ClaimsPrincipal claimsPrincipal
                        ) =>
                        {
                            var id = int.Parse(claimsPrincipal.Identity.GetUserId());
                            var oid = appUserManager.GetUserByIdAsync(id).Result.OrganizationId;
                            var res = await sender.Send(model with { OrganizationId = oid });
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "AddStudent",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageStudents.ToString()
                )
            )
            .WithDescription("Returns new student id")
            .Produces<OperationResult<int>>()
            .Accepts<AddStudentCommand>("multipart/form-data")
            .DisableAntiforgery();

        // edit student
        app.MapEndpoint(
                builder =>
                    builder.MapPatch(
                        $"{RoutePrefix}",
                        async (
                            UpdateStudentCommand model,
                            ISender sender,
                            IAppUserManager appUserManager
                        ) =>
                        {
                            var res = await sender.Send(model);
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "UpdateStudent",
                Tag
            )
            .RequireAuthorization(s =>
                s.RequireClaim(
                    ConstantPolicies.DynamicPermission,
                    DynamicPermission.IsManager.ToString(),
                    DynamicPermission.CanManageStudents.ToString()
                )
            )
            .WithOpenApi(op =>
                Utl.WriteOpenApiErrors(
                    op,
                    RawdahErrors.UserNotFound,
                    RawdahErrors.FormatNotSupported
                )
            )
            .Produces<OperationResult<bool>>();

        // get a student
        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{RoutePrefix}",
                        async (int studentId, ISender sender) =>
                        {
                            var res = await sender.Send(new GetStudentQuery(studentId));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "GetStudent",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<GetStudentQueryResponse>>();

        // get a student
        app.MapEndpoint(
                builder =>
                    builder.MapDelete(
                        $"{RoutePrefix}",
                        async (int studentId, ISender sender) =>
                        {
                            var res = await sender.Send(new RemoveStudentCommand(studentId));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "DeleteStudent",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.UserNotFound))
            .Produces<OperationResult<bool>>();

        // get all students

        app.MapEndpoint(
                builder =>
                    builder.MapGet(
                        $"{RoutePrefix}All",
                        async (
                            ISender sender,
                            ClaimsPrincipal claimsPrincipal,
                            IAppUserManager appUserManager
                        ) =>
                        {
                            var id = int.Parse(claimsPrincipal.Identity.GetUserId());
                            var orgId = appUserManager.GetUserByIdAsync(id).Result.OrganizationId;
                            var res = await sender.Send(new GetStudentsByOrganizationQuery(orgId));
                            return res.ToEndpointResult();
                        }
                    ),
                Version,
                "GetAllStudent",
                Tag
            )
            .RequireAuthorization()
            .WithOpenApi(op => Utl.WriteOpenApiErrors(op, RawdahErrors.OrganizationNotFound))
            .Produces<OperationResult<List<GetStudentQueryResponse>>>();
    }
}
