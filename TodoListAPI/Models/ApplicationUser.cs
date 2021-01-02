using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using Microsoft.AspNetCore.Identity;

namespace TodoListAPI.Models
{
    public class ApplicationUser : IdentityUser, IIdentifiable<string>
    {
        [NotMapped]
        public string StringId { get => Id; set => Id = value; }

        [Attr]
        public string FirstName { get; set; }

        [Attr]
        public string LastName { get; set; }

        [HasMany]
        public virtual ISet<TodoItem> TodoItems { get; set; }
    }
}
