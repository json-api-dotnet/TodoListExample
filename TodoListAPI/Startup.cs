using System;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using JsonApiDotNetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Services;

namespace TodoListAPI
{
    public class Startup
    {
        private readonly string _connectionString;

        public Startup(IConfiguration configuration)
        {
            _connectionString = configuration["Data:DefaultConnection"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors();

            services.AddJsonApi(
                options =>
                {
                    options.Namespace = "api/v1";
                    options.IncludeExceptionStackTraceInErrors = true;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new KebabCaseNamingStrategy()
                    };
                }, 
                discovery => discovery.AddCurrentAssembly());

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(_connectionString);
                options.UseOpenIddict();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    // Register the Entity Framework stores.
                    options.UseEntityFrameworkCore(builder =>
                    {
                        builder.UseDbContext<AppDbContext>();
                    });
                })
                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();

                    options.AcceptAnonymousClients();

                    // Enable the token endpoint (required to use the password flow).
                    options.EnableTokenEndpoint("/connect/token");

                    // Allow client applications to use the grant_type=password flow.
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();
                })
                .AddValidation();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation($"Starting application in {env.EnvironmentName} environment");

            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseJsonApi();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            SeedDatabaseAsync(context, userManager).Wait();
        }

        private async Task SeedDatabaseAsync(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

            var user = new ApplicationUser
            {
                UserName = "guest",
                Email = "jaredcnance@gmail.com"
            };

            var result = await userManager.CreateAsync(user, "Guest1!");
            if (!result.Succeeded)
            {
                throw new Exception("Could not create default user.");
            }

            context.TodoItems.Add(new TodoItem
            {
                OwnerId = user.Id,
                Description = "owned"
            });

            context.TodoItems.Add(new TodoItem
            {
                Description = "not owned"
            });

            await context.SaveChangesAsync();
        }
    }
}
