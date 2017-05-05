using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JsonApiDotNetCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TodoListAPI.Models
{
    public class ApplicationUser : IdentityUser, IIdentifiable<string>
    {
        [NotMapped]
        public string StringId { get => this.Id; set => Id = value; }

        [Attr("first-name")]
        public string FirstName { get; set; }
        
        [Attr("last-name")]
        public string LastName { get; set; }

        [HasMany("todo-items")]
        public virtual List<TodoItem> TodoItems { get; set; }
    }
}