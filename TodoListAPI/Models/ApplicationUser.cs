using System.ComponentModel.DataAnnotations.Schema;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using Microsoft.AspNetCore.Identity;

namespace TodoListAPI.Models;

public sealed class ApplicationUser : IdentityUser, IIdentifiable<string>
{
    [NotMapped]
    public string? StringId
    {
        get => Id;
        set => Id = value;
    }

    [NotMapped]
    public string? LocalId { get; set; }

    [Attr]
    public string? FirstName { get; set; }

    [Attr]
    public string? LastName { get; set; }

    [HasMany]
    public ISet<TodoItem> TodoItems { get; set; } = new HashSet<TodoItem>();
}
