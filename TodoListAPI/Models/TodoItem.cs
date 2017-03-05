using JsonApiDotNetCore.Models;

namespace TodoListAPI.Models
{
    public class TodoItem : Identifiable<int>
    {
        [Attr("description")]
        public string Description { get; set; }

        public int OwnerId { get; set; }
        [HasOne("person")]
        public virtual Person Owner { get; set; }
    }
}