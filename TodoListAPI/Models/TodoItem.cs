using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace TodoListAPI.Models;

[Resource]
public sealed class TodoItem : Identifiable<long>
{
    [Attr]
    public string Description { get; set; } = null!;

    [HasOne]
    public ApplicationUser Owner { get; set; } = null!;
}
