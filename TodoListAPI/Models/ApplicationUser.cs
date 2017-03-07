using System.Collections.Generic;
using JsonApiDotNetCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TodoListAPI.Models
{
    public class ApplicationUser : IdentityUser, IIdentifiable<string>
    {
        object IIdentifiable.Id
        {
            get { return Id; }
            set { Id = value.ToString(); }
        }

        [Attr("first-name")]
        public string FirstName { get; set; }
        
        [Attr("last-name")]
        public string LastName { get; set; }

        [HasMany("todo-items")]
        public virtual List<TodoItem> TodoItems { get; set; }
    }
}