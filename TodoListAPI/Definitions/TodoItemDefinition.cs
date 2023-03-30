using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Resources;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using TodoListAPI.Models;

namespace TodoListAPI.Definitions;

public sealed class TodoItemDefinition : JsonApiResourceDefinition<TodoItem, long>
{
    private readonly IJsonApiRequest _request;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public TodoItemDefinition(IResourceGraph resourceGraph, IJsonApiRequest request, IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
        : base(resourceGraph)
    {
        _request = request;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public override async Task OnPrepareWriteAsync(TodoItem todoItem, WriteOperationKind writeOperation, CancellationToken cancellationToken)
    {
        if (writeOperation == WriteOperationKind.CreateResource)
        {
            todoItem.Owner = await GetCurrentUserAsync();
        }
    }

    private async Task<ApplicationUser> GetCurrentUserAsync()
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(_httpContextAccessor.HttpContext?.User.GetClaim(OpenIddictConstants.Claims.Subject));

        if (user == null)
        {
            throw new InvalidOperationException("Could not find current user.");
        }

        return user;
    }

    public override void OnDeserialize(TodoItem todoItem)
    {
        if (_request.WriteOperation == WriteOperationKind.CreateResource)
        {
            // Prevent ASP.NET ModelState validation error, because required Owner relationship was not sent.
            todoItem.Owner = new ApplicationUser();
        }
    }
}
