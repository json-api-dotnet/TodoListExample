using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Resources;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using TodoListAPI.Models;

namespace TodoListAPI.Definitions;

public sealed class TodoItemDefinition(
    IResourceGraph resourceGraph, IJsonApiRequest request, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    : JsonApiResourceDefinition<TodoItem, long>(resourceGraph)
{
    public override async Task OnPrepareWriteAsync(TodoItem todoItem, WriteOperationKind writeOperation, CancellationToken cancellationToken)
    {
        if (writeOperation == WriteOperationKind.CreateResource)
        {
            todoItem.Owner = await GetCurrentUserAsync();
        }
    }

    private async Task<ApplicationUser> GetCurrentUserAsync()
    {
        string? userId = httpContextAccessor.HttpContext?.User.GetClaim(OpenIddictConstants.Claims.Subject);
        if (userId == null)
        {
            throw new InvalidOperationException("Could not find current user.");
        }

        ApplicationUser? user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException("Could not find current user.");
        }

        return user;
    }

    public override void OnDeserialize(TodoItem todoItem)
    {
        if (request.WriteOperation == WriteOperationKind.CreateResource)
        {
            // Prevent ASP.NET ModelState validation error, because required Owner relationship was not sent.
            todoItem.Owner = new ApplicationUser();
        }
    }
}
