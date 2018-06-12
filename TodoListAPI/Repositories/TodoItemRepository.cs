using System.Linq;
using System.Threading.Tasks;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Services;

namespace TodoListAPI.Repositories
{
    public class TodoItemRepository : DefaultEntityRepository<TodoItem>
    {
        private readonly ILogger _logger;
        private readonly IAuthenticationService _authenticationService;

        public TodoItemRepository(
            IDbContextResolver contextResolver,
            ILoggerFactory loggerFactory,
            IJsonApiContext jsonApiContext,
            IAuthenticationService authenticationService)
        : base(loggerFactory, jsonApiContext, contextResolver)
        {
            _logger = loggerFactory.CreateLogger<TodoItemRepository>();
            _authenticationService = authenticationService;
        }

        public override IQueryable<TodoItem> Get()
        {
            return base.Get().Where(e => e.OwnerId == _authenticationService.GetUserId());
        }

        public override async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            todoItem.OwnerId = _authenticationService.GetUserId();
            return await base.CreateAsync(todoItem);
        }
    }
}
