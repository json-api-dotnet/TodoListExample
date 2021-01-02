using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Models;

namespace TodoListAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public string GetUserId()
        {
             return _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
        }
    }
}
