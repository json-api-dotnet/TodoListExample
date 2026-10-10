using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict.Abstractions;
using TodoListAPI.Models;

namespace TodoListAPI.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TodoItem>()
            .HasQueryFilter(todoItem => !IsInHttpRequest() || todoItem.Owner.Id == GetCurrentUserId());

        AdjustDeleteBehaviorForJsonApi(builder);
    }

    private bool IsInHttpRequest()
    {
        return httpContextAccessor.HttpContext != null;
    }

    private string GetCurrentUserId()
    {
        Claim? claim = httpContextAccessor.HttpContext?.User.FindFirst(OpenIddictConstants.Claims.Subject);

        if (claim != null)
        {
            return claim.Value;
        }

        throw new InvalidOperationException("Could not find current user.");
    }

    private static void AdjustDeleteBehaviorForJsonApi(ModelBuilder builder)
    {
        foreach (IMutableForeignKey foreignKey in builder.Model.GetEntityTypes()
            .SelectMany(entityType => entityType.GetForeignKeys()))
        {
            if (foreignKey.DeleteBehavior == DeleteBehavior.ClientSetNull)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.SetNull;
            }

            if (foreignKey.DeleteBehavior == DeleteBehavior.ClientCascade)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }
        }
    }
}
