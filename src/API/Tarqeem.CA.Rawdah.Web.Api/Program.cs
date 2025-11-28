using System.Diagnostics;
using Carter;
using Tarqeem.CA.Application.ServiceConfiguration;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.Infrastructure.CrossCutting.Logging;
using Tarqeem.CA.Infrastructure.Identity.Identity.Dtos;
using Tarqeem.CA.Infrastructure.Identity.Jwt;
using Tarqeem.CA.Infrastructure.Identity.ServiceConfiguration;
using Tarqeem.CA.Infrastructure.Persistence.ServiceConfiguration;
using Tarqeem.CA.SharedKernel.Extensions;
using Tarqeem.CA.WebFramework.Filters;
using Tarqeem.CA.WebFramework.Middlewares;
using Tarqeem.CA.WebFramework.ServiceConfiguration;
using Tarqeem.CA.WebFramework.Swagger;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP port
    // Comment or remove any HTTPS configuration
    // options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps());
});



var configuration = builder.Configuration;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

builder.Services.Configure<IdentitySettings>(configuration.GetSection(nameof(IdentitySettings)));

var identitySettings = configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(OkResultAttribute));
    options.Filters.Add(typeof(NotFoundResultAttribute));
    options.Filters.Add(typeof(ContentResultFilterAttribute));
    options.Filters.Add(typeof(ModelStateValidationAttribute));
    options.Filters.Add(typeof(BadRequestResultFilterAttribute));
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddCarter(configurator: configurator => { configurator.WithEmptyValidators(); });

builder.Services.AddApplicationServices()
    .RegisterIdentityServices(identitySettings)
    .AddPersistenceServices(configuration)
    .AddWebFrameworkServices();

builder.Services.RegisterValidatorsAsServices();
builder.Services.AddExceptionHandler<ExceptionHandler>();


builder.Services.AddAutoMapper(expression => { expression.AddMaps(typeof(User), typeof(JwtService)); });

const string devCros = "AllowDevelopment";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: devCros,
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("http://localhost",
                "http://localhost:3000",
                "https://localhost:3000",

                "http://localhost:5173",
                "https://localhost:5173",

                "https://localhost"
            ).AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();


app.UseCors(devCros);

await app.ApplyMigrationsAsync();
await app.SeedDefaultUsersAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler(_ => { });
app.UseSwaggerAndUI();

app.MapCarter();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


await app.RunAsync();
