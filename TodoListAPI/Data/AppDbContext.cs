using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using TodoListAPI.Models;

namespace TodoListAPI.Data;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TodoItem>().HasQueryFilter(todoItem => !IsInHttpRequest() || todoItem.Owner.Id == GetCurrentUserId());
    }

    private bool IsInHttpRequest()
    {
        return _httpContextAccessor.HttpContext != null;
    }

    private string GetCurrentUserId()
    {
        Claim? claim = _httpContextAccessor.HttpContext?.User.FindFirst(OpenIddictConstants.Claims.Subject);

        if (claim != null)
        {
            return claim.Value;
        }

        throw new InvalidOperationException("Could not find current user.");
    }
}
