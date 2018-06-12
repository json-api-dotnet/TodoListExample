using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using JsonApiDotNetCore.Extensions;
using JsonApiDotNetCore.Data;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Models;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Threading.Tasks;
using System;
using TodoListAPI.Repositories;
using TodoListAPI.Services;
using Microsoft.AspNetCore.Http;

namespace TodoListAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors();
            services.AddMvc();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(GetConnectionString());
                opt.UseOpenIddict();
            }, ServiceLifetime.Transient);

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

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEntityRepository<TodoItem>, TodoItemRepository>();

            services.AddJsonApi<AppDbContext>(opt => opt.Namespace = "api/v1");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation($"Starting application in {env.EnvironmentName} environment");

            if (env.IsDevelopment())
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });

            app.UseAuthentication();

            app.UseJsonApi();

            SeedDatabase(context, userManager).Wait();
        }

        private async Task SeedDatabase(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            await context.Database.MigrateAsync();
            var usersExist = await context.Users.AnyAsync();
            if (usersExist == false)
            {
                var user = new ApplicationUser
                {
                    UserName = "guest",
                    Email = "jaredcnance@gmail.com"
                };

                var result = await userManager.CreateAsync(user, "Guest1!");

                if (!result.Succeeded) throw new Exception("Could not create default user");

                context.TodoItems.Add(new TodoItem
                {
                    Owner = user,
                    Description = "owned"
                });

                context.TodoItems.Add(new TodoItem
                {
                    Description = "not owned"
                });

                context.SaveChanges();
            }
        }

        private string GetConnectionString()
        {
            return Configuration["ConnectionString"];
        }
    }
}
