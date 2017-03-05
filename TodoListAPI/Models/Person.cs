using System.Collections.Generic;
using JsonApiDotNetCore.Models;

namespace TodoListAPI.Models
{
    public class Person : Identifiable
    {
        [Attr("first-name")]
        public string FirstName { get; set; }
        
        [Attr("last-name")]
        public string LastName { get; set; }

        [HasMany("todo-items")]
        public virtual List<TodoItem> TodoItems { get; set; }
    }
}