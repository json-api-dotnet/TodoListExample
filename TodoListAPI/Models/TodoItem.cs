using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace TodoListAPI.Models
{
    public class TodoItem : Identifiable
    {
        [Attr]
        public string Description { get; set; }

        public string OwnerId { get; set; }

        [HasOne]
        public virtual ApplicationUser Owner { get; set; }
    }
}
