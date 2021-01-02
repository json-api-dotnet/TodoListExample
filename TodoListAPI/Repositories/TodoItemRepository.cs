using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;
using Microsoft.Extensions.Logging;
using TodoListAPI.Models;
using TodoListAPI.Services;

namespace TodoListAPI.Repositories
{
    public class TodoItemRepository : EntityFrameworkCoreRepository<TodoItem>
    {
        private readonly IAuthenticationService _authenticationService;

        public TodoItemRepository(IAuthenticationService authenticationService, ITargetedFields targetedFields,
            IDbContextResolver contextResolver, IResourceGraph resourceGraph, IResourceFactory resourceFactory,
            IEnumerable<IQueryConstraintProvider> constraintProviders, ILoggerFactory loggerFactory)
            : base(targetedFields, contextResolver, resourceGraph, resourceFactory, constraintProviders, loggerFactory)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        protected override IQueryable<TodoItem> GetAll()
        {
            return base.GetAll().Where(todoItem => todoItem.Owner.Id == _authenticationService.GetUserId());
        }

        public override async Task<TodoItem> GetForCreateAsync(int id, CancellationToken cancellationToken)
        {
            var todoItem = await base.GetForCreateAsync(id, cancellationToken);
            todoItem.OwnerId = _authenticationService.GetUserId();
            return todoItem;
        }
    }
}
