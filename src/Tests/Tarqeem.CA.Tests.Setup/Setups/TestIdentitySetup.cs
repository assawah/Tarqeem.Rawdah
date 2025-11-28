using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.Infrastructure.Identity.Identity.Dtos;
using Tarqeem.CA.Infrastructure.Identity.Identity.Extensions;
using Tarqeem.CA.Infrastructure.Identity.Identity.Manager;
using Tarqeem.CA.Infrastructure.Identity.Identity.Store;
using Tarqeem.CA.Infrastructure.Identity.Jwt;
using Tarqeem.CA.Infrastructure.Identity.UserManager;
using Tarqeem.CA.Infrastructure.Persistence;
using Tarqeem.CA.Infrastructure.Persistence.Repositories.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tarqeem.CA.Tests.Setup.Setups;

public abstract class TestIdentitySetup
{
    protected IAppUserManager TestAppUserManager { get; }
    protected AppRoleManager TestAppRoleManager { get; }
    public AppSignInManager TestSignInManager { get; }
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public IAppUserManager User { get; }
    public IJwtService JwtService { get; }

    protected TestIdentitySetup()
    {
        var serviceCollection = new ServiceCollection();

        var connection = new SqliteConnection("DataSource=:memory:");

        serviceCollection.AddLogging();

        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));

        var context = serviceCollection.BuildServiceProvider().GetService<ApplicationDbContext>();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();


        serviceCollection.AddIdentity<User, Role>(options =>
        {
            options.Stores.ProtectPersonalData = false;

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireUppercase = false;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;
            options.User.RequireUniqueEmail = false;

        }).AddUserStore<AppUserStore>()
            .AddRoleStore<RoleStore>().
            AddUserManager<AppUserManager>().
            AddRoleManager<AppRoleManager>().
            AddDefaultTokenProviders().
            AddSignInManager<AppSignInManager>()
            .AddDefaultTokenProviders()
            .AddPasswordlessLoginTotpTokenProvider();

        serviceCollection.Configure<IdentitySettings>(settings =>
        {
            settings.Audience = "CleanArc.Unit.Tests";
            settings.ExpirationMinutes = 5;
            settings.Issuer = "CleanArc.Unit.Tests";
            settings.NotBeforeMinutes = 0;
            settings.SecretKey = "ShouldBe-LongerThan-16Char-SecretKey";
            settings.Encryptkey = "16CharEncryptKey";
        });

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IJwtService, JwtService>();
        serviceCollection.AddScoped<IAppUserManager, AppUserManagerImplementation>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        TestAppUserManager = serviceProvider.GetRequiredService<IAppUserManager>();
        TestAppRoleManager = serviceProvider.GetRequiredService<AppRoleManager>();
        TestSignInManager = serviceProvider.GetRequiredService<AppSignInManager>();
        JwtService = serviceProvider.GetRequiredService<IJwtService>();
    }
}
