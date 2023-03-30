using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace TodoListAPI.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public partial class TodoItemsController
{
}
