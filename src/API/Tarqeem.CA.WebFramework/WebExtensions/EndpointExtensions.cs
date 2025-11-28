using Tarqeem.CA.Application.Models.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tarqeem.CA.WebFramework.EndpointFilters;

namespace Tarqeem.CA.WebFramework.WebExtensions;

public static class EndpointExtensions
{
    public static IResult ToEndpointResult<TModel>(this OperationResult<TModel> result)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(OperationResult<TModel>));

        if (result.IsSuccess) return result.Result is bool ? Results.Ok() : Results.Ok(result.Result);

        return result.IsNotFound ? Results.NotFound() : Results.BadRequest(result);
    }


    public static RouteHandlerBuilder MapEndpoint(this IEndpointRouteBuilder app
        , Func<IEndpointRouteBuilder, RouteHandlerBuilder> handler
        , double apiVersion
        , string name
        , params string[] tags)
    {
        var versionedEndpoint = app.NewVersionedApi();

        return handler(versionedEndpoint)
            .WithOpenApi()
            .HasApiVersion(apiVersion)
            .AddEndpointFilter<OkResultEndpointFilter>()
            .AddEndpointFilter<NotFoundResultEndpointFilter>()
            .AddEndpointFilter<BadRequestResultEndpointFilter>()
            .AddEndpointFilter<ModelStateValidationEndpointFilter>()
            .WithName(name)
            .WithTags(tags);
    }
}
