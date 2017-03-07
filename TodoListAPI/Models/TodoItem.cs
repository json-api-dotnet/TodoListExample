using JsonApiDotNetCore.Models;

namespace TodoListAPI.Models
{
    public class TodoItem : Identifiable<int>
    {
        [Attr("description")]
        public string Description { get; set; }

        public string OwnerId { get; set; }

        [HasOne("owner")]
        public virtual ApplicationUser Owner { get; set; }
    }
}