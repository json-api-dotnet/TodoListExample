using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Models;

namespace TodoListAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            _context = context.HttpContext;
            _userManager = userManager;
        }

        public string GetUserId()
        {
             return _userManager.GetUserId(_context.User);
        }
    }
}
