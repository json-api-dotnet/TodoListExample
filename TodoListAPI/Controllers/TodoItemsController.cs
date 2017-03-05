using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    public class TodoItemsController : JsonApiController<TodoItem>
    {
         public TodoItemsController(
            IJsonApiContext jsonApiContext,
            IEntityRepository<TodoItem> entityRepository,
            ILoggerFactory loggerFactory) 
            : base(jsonApiContext, entityRepository, loggerFactory)
        { }
    }
}