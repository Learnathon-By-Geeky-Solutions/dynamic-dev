using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginService> _logger;


        private static string RedactEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return "REDACTED";
            var parts = email.Split('@');
            return $"{parts[0][0]}***@{parts[1]}";
        }
        public LoginService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<LoginService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(bool Success, string ErrorMessage, string? RedirectUrl)> AuthenticateUserAsync(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Authentication failed: Email is null or empty.");
                return (false, "Email cannot be empty.", null);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Authentication failed: Password is null or empty.");
                return (false, "Password cannot be empty.", null);
            }

            try
            {
                email = email.Replace("\n", "").Replace("\r", "");
                _logger.LogInformation("Attempting to authenticate user with email: {Email}", RedactEmail(email));
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    email = email.Replace("\n", "").Replace("\r", "");
                    _logger.LogWarning("Authentication failed: User with email {Email} not found.", RedactEmail(email));
                    return (false, "User not found!", null);
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
                if (!result.Succeeded)
                {
                    email = email.Replace("\n", "").Replace("\r", "");
                    _logger.LogWarning("Authentication failed: Invalid login attempt for user with email {Email}.", RedactEmail(email));
                    return (false, "Invalid login attempt!", null);
                }

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(role => IsAdminRole(role)))
                {
                    email = email.Replace("\n", "").Replace("\r", "");
                    _logger.LogInformation("Authentication successful: User with email {Email} is an admin.", RedactEmail(email));
                    return (true, "", "/Admin/AdminDashboard/Index");
                }

                _logger.LogInformation("Authentication successful: User with email {Email} has no defined role.", RedactEmail(email));
                return (true, "User role is not defined", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating user with email: {Email}", RedactEmail(email));
                throw new InvalidOperationException($"An error occurred while authenticating user with email: {email}.", ex);
            }
        }

        private static bool IsAdminRole(string role)
        {
            var adminRoles = new HashSet<string> { "admin", "agencyManager", "hotelManager", "busManager", "carManager" };
            return adminRoles.Contains(role);
        }
    }
}
