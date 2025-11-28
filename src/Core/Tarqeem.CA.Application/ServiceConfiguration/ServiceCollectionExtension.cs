using System.Reflection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Tarqeem.CA.Application.Common;

namespace Tarqeem.CA.Application.ServiceConfiguration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
            options.Namespace = "Tarqeem.CA.Application.Mediator";
        });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidateCommandBehavior<,>));
        //services.AddMediator(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(expression =>
        {
            expression.AddMaps(Assembly.GetExecutingAssembly());
        });


        return services;
    }

   
}