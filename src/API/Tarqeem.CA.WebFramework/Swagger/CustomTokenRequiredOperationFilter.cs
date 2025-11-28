using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tarqeem.CA.WebFramework.Swagger;

public class CustomTokenRequiredOperationFilter : IOperationFilter
{
    private readonly SecurityRequirementsOperationFilter<RequireTokenWithoutAuthorizationAttribute> _filter;

    public CustomTokenRequiredOperationFilter()
    {
        this._filter =
            new SecurityRequirementsOperationFilter<RequireTokenWithoutAuthorizationAttribute>(
                _ => Array.Empty<string>(), false);
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context) => this._filter.Apply(operation, context);


}
