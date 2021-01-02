using AspNet.Security.OAuth.Validation;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class TodoItemsController : JsonApiController<TodoItem>
    {
        public TodoItemsController(IJsonApiOptions options, ILoggerFactory loggerFactory,
            IResourceService<TodoItem> resourceService)
            : base(options, loggerFactory, resourceService)
        {
        }
    }
}