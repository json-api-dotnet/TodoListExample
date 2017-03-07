using System.Linq;
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
        private readonly AppDbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public TodoItemRepository(AppDbContext context,
            ILoggerFactory loggerFactory,
            IJsonApiContext jsonApiContext,
            IAuthenticationService authenticationService)
        : base(context, loggerFactory, jsonApiContext)
        { 
            _context = context;
            _logger = loggerFactory.CreateLogger<TodoItemRepository>();
            _authenticationService = authenticationService;
        }

        public override IQueryable<TodoItem> Get()
        {
            return base.Get().Where(e => e.OwnerId == _authenticationService.GetUserId());
        }
    }
}
