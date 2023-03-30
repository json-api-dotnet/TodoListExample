using JorgeSerrano.Json;
using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoListAPI.Data;
using TodoListAPI.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseNpgsql(connectionString);

#if DEBUG
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.ConfigureWarnings(warningsBuilder => warningsBuilder.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning));
#endif

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

builder.Services.AddJsonApi<AppDbContext>(options =>
{
    options.Namespace = "api/v1";
    options.UseRelativeLinks = true;
    options.IncludeTotalResourceCount = true;
    options.SerializerOptions.PropertyNamingPolicy = new JsonKebabCaseNamingPolicy();

#if DEBUG
    options.SerializerOptions.WriteIndented = true;
    options.IncludeExceptionStackTraceInErrors = true;
    options.IncludeRequestBodyInErrors = true;
#endif
}, discovery => discovery.AddCurrentAssembly());

// Register the Identity services.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        options.UseEntityFrameworkCore().UseDbContext<AppDbContext>();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the token endpoint.
        options.SetTokenEndpointUris("connect/token");

        // Enable the password and the refresh token flows.
        options.AllowPasswordFlow().AllowRefreshTokenFlow();

        // Uncomment these to test token refresh/expiration.
        //options.SetAccessTokenLifetime(TimeSpan.FromSeconds(15));
        //options.SetRefreshTokenLifetime(TimeSpan.FromSeconds(30));

        // Accept anonymous clients (i.e clients that don't send a client_id).
        options.AcceptAnonymousClients();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore().EnableTokenEndpointPassthrough()

#if DEBUG
            // Disable HTTPS requirement. Note this sends the password in plaintext!
            .DisableTransportSecurityRequirement();
#endif
    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

if (builder.Environment.IsDevelopment())
{
    app.UseCors(policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseJsonApi();

app.MapControllers();

await CreateDatabaseAsync(app.Services);

app.Run();

static async Task CreateDatabaseAsync(IServiceProvider serviceProvider)
{
    await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // By default, the Ember login session survives API restarts. Uncomment the next line to start clean on each run.
    // await dbContext.Database.EnsureDeletedAsync();

    if (await dbContext.Database.EnsureCreatedAsync())
    {
        ApplicationUser guest = await CreateUser("guest", "guest@email.com", "Guest1!", scope.ServiceProvider);
        ApplicationUser john = await CreateUser("john", "john@email.com", "P@ssw0rd!", scope.ServiceProvider);

        dbContext.TodoItems.AddRange(new TodoItem
        {
            Owner = guest,
            Description = "owned-by-guest"
        }, new TodoItem
        {
            Owner = john,
            Description = "owned-by-john"
        });

        await dbContext.SaveChangesAsync();
    }
}

static async Task<ApplicationUser> CreateUser(string userName, string email, string password, IServiceProvider serviceProvider)
{
    var user = new ApplicationUser
    {
        UserName = userName,
        Email = email
    };

    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    IdentityResult result = await userManager.CreateAsync(user, password);

    if (!result.Succeeded)
    {
        throw new Exception("Could not create user.");
    }

    return user;
}
