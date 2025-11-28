using Carter;
using Tarqeem.CA.WebFramework.WebExtensions;
using Mediator;
using Tarqeem.CA.Application.Features.Manager.Commands.AddManagerCommand;
using Tarqeem.CA.Application.Features.Users.Queries;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Models.Jwt;
using Tarqeem.CA.SharedKernel;

namespace Tarqeem.CA.Rawdah.Web.Api.Endpoints;

// ReSharper disable once UnusedType.Global
public class AdminEndpoints : ICarterModule
{
    private readonly string _routePrefix = "/api/v{version:apiVersion}/Admin/";
    private readonly double _version = 1.1;
    private readonly string _tag = "AdminManager";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapEndpoint(
            builder => builder.MapPost($"{_routePrefix}Login"
                , async (UserGetTokenQuery model, ISender sender) => (await sender.Send(model)).ToEndpointResult())
            , _version
            , "AdminLogin"
            , _tag).WithOpenApi(op =>
            Utl.WriteOpenApiErrors(op,
                RawdahErrors.UsernameNotFound,
                RawdahErrors.PasswordInvalid,
                RawdahErrors.LockedOut)).Produces<OperationResult<AccessToken>>();

        app.MapEndpoint(builder => builder.MapPost($"{_routePrefix}AddNewManager"
                , async (AddManagerCommand model, ISender sender) => (await sender.Send(model)).ToEndpointResult()),
            _version,
            "AdminAddNewManager",
            _tag).RequireAuthorization(r => r.RequireRole("admin")).WithOpenApi(op =>
            Utl.WriteOpenApiErrors(op, RawdahErrors.UsernameExists)).Produces<OperationResult<bool>>();
    }
}