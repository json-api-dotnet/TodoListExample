using AspNet.Security.OAuth.Validation;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class TodoItemsController : JsonApiController<TodoItem>
    {
         public TodoItemsController(
            IJsonApiContext jsonApiContext,
            IResourceService<TodoItem> resourceService,
            ILoggerFactory loggerFactory) 
            : base(jsonApiContext, resourceService, loggerFactory)
        { }
    }
}