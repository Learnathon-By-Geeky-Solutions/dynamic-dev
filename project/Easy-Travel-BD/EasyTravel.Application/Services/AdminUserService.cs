using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminUserService> _logger;

        public AdminUserService(UserManager<User> userManager, ILogger<AdminUserService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private static string RedactEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return "REDACTED";
            var parts = email.Split('@');
            return $"{parts[0][0]}***@{parts[1]}";
        }
        public User GetUserInstance()
        {
            _logger.LogInformation("Creating a new user instance.");
            return new User();
        }

        public async Task<(bool Success, string ErrorMessage)> CreateAsync(User user, string password)
        {
            if (user == null)
            {
                _logger.LogWarning("Attempted to create a null user entity.");
                return (false, "User entity cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Attempted to create a user with an empty password.");
                return (false, "Password cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Creating a new user with email: {Email}", user.Email);
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to create user with email: {Email}. Errors: {Errors}", user.Email, errorMessage);
                    return (false, errorMessage);
                }

                _logger.LogInformation("Successfully created user with email: {Email}", user.Email);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user with email: {Email}", user.Email);
                throw new InvalidOperationException($"An error occurred while creating the user with email: {user.Email}.", ex);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAsync(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("Attempted to update a null user entity.");
                return (false, "User entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating user with ID: {Id}", user.Id);
                user.UserName = user.Email;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to update user with ID: {Id}. Errors: {Errors}", user.Id, errorMessage);
                    return (false, errorMessage);
                }

                _logger.LogInformation("Successfully updated user with ID: {Id}", user.Id);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user with ID: {Id}", user.Id);
                throw new InvalidOperationException($"An error occurred while updating the user with ID: {user.Id}.", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for deletion.");
                throw new ArgumentException("User ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete user with ID: {Id}", id);
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {Id} not found for deletion.", id);
                    return;
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to delete user with ID: {Id}. Errors: {Errors}", id, errorMessage);
                    throw new InvalidOperationException($"Failed to delete user with ID: {id}. Errors: {errorMessage}");
                }

                _logger.LogInformation("Successfully deleted user with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the user with ID: {id}.", ex);
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                return _userManager.Users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                throw new InvalidOperationException("An error occurred while fetching all users.", ex);
            }
        }

        public async Task<User?> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for retrieval.");
                throw new ArgumentException("User ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching user with ID: {Id}", id);
                var user = await _userManager.FindByIdAsync(id.ToString());
                return user; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the user with ID: {id}.", ex);
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var redactedEmail = RedactEmail(email);
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Invalid email provided for getting user.");
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            try
            {
                _logger.LogInformation("Fetching user with email: {Email}", redactedEmail);
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user with email: {Email}", redactedEmail);
                throw new InvalidOperationException($"An error occurred while fetching the user with email: {redactedEmail}.", ex);
            }
        }

        public async Task<bool> IsExist(string email)
        {
            var redactedEmail = RedactEmail(email);
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Invalid email provided for existence check.");
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            try
            {
                _logger.LogInformation("Checking existence of user with email: {Email}", redactedEmail);
                return await _userManager.FindByEmailAsync(email) != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking existence of user with email: {Email}", redactedEmail);
                throw new InvalidOperationException($"An error occurred while checking existence of user with email: {redactedEmail}.", ex);
            }
        }

        public async Task<PagedResult<User>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            try
            {
                _logger.LogInformation("Fetching paginated users for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);

                // Fetch all users
                var totalItems = _userManager.Users.Count();

                // Apply pagination
                var users = _userManager.Users
                    .OrderBy(u => u.UserName) // Sort by username
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Create the paginated result
                var result = new PagedResult<User>
                {
                    Items = users,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated users.");
                throw new InvalidOperationException("An error occurred while fetching paginated users.", ex);
            }
        }
    }
}


