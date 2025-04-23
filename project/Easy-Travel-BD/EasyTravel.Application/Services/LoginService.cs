using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<(bool Success, string ErrorMessage, string? RedirectUrl)> AuthenticateUserAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return (false, "User not found!", null);

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            if (!result.Succeeded) return (false, "Invalid login attempt!", null);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(role => IsAdminRole(role))) return (true, "", "/Admin/AdminDashboard/Index");

            return (true, "User role is not defined", null);
        }

        private static bool IsAdminRole(string role)
        {
            var adminRoles = new HashSet<string> { "admin", "agencyManager", "hotelManager", "busManager", "carManager" };
            return adminRoles.Contains(role);
        }
    }
}
