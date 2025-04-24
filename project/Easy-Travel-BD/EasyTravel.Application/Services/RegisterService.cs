using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterService> _logger;

        public RegisterService(UserManager<User> userManager, ILogger<RegisterService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(User user, string password)
        {
            if (user == null)
            {
                _logger.LogWarning("Attempted to register a null User entity.");
                throw new ArgumentNullException(nameof(user), "User entity cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Attempted to register a user with an empty password.");
                return (false, "Password cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Registering a new user with email: {Email}", user.Email);
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to register user with email: {Email}. Errors: {Errors}", user.Email, errorMessage);
                    return (false, errorMessage);
                }

                _logger.LogInformation("Successfully registered user with email: {Email}. Assigning 'client' role.", user.Email);
                await _userManager.AddToRoleAsync(user, "client");
                _logger.LogInformation("Successfully assigned 'client' role to user with email: {Email}", user.Email);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user with email: {Email}", user.Email);
                throw new InvalidOperationException($"An error occurred while registering the user with email: {user.Email}.", ex);
            }
        }
    }
}