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
    public class AdminUserRoleService : IAdminUserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AdminUserRoleService> _logger;

        public AdminUserRoleService(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AdminUserRoleService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IdentityResult> CreateAsync(Guid UserId, Guid RoleId)
        {
            if (UserId == Guid.Empty || RoleId == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID or role ID provided for role assignment.");
                return IdentityResult.Failed(new IdentityError { Description = "User ID or Role ID cannot be empty." });
            }

            try
            {
                _logger.LogInformation("Assigning role with ID: {RoleId} to user with ID: {UserId}", RoleId, UserId);
                var user = await _userManager.FindByIdAsync(UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found.", UserId);
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                var role = await _roleManager.FindByIdAsync(RoleId.ToString());
                if (role == null)
                {
                    _logger.LogWarning("Role with ID: {RoleId} not found.", RoleId);
                    return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
                }

                if (await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    _logger.LogInformation("User with ID: {UserId} is already in role: {RoleName}", UserId, role.Name);
                    return IdentityResult.Success;
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name!);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Successfully assigned role with ID: {RoleId} to user with ID: {UserId}", RoleId, UserId);
                }
                else
                {
                    _logger.LogWarning("Failed to assign role with ID: {RoleId} to user with ID: {UserId}. Errors: {Errors}", RoleId, UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning role with ID: {RoleId} to user with ID: {UserId}", RoleId, UserId);
                throw new InvalidOperationException($"An error occurred while creating the role with user Id:{UserId} or role Id: {RoleId}.", ex);
            }
        }

        public async Task<IdentityResult> UpdateAsync(Guid UserId, Guid RoleId)
        {
            if (UserId == Guid.Empty || RoleId == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID or role ID provided for role update.");
                return IdentityResult.Failed(new IdentityError { Description = "User ID or Role ID cannot be empty." });
            }

            try
            {
                _logger.LogInformation("Updating roles for user with ID: {UserId}", UserId);
                var user = await _userManager.FindByIdAsync(UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found.", UserId);
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    _logger.LogWarning("Failed to remove existing roles for user with ID: {UserId}. Errors: {Errors}", UserId, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    return removeResult;
                }

                return await CreateAsync(UserId, RoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating roles for user with ID: {UserId}", UserId);
                throw new InvalidOperationException($"An error occurred while creating the role with user Id:{UserId} or role Id: {RoleId}.", ex);
            }
        }

        public async Task<IdentityResult> DeleteAsync(Guid UserId, Guid RoleId)
        {
            if (UserId == Guid.Empty || RoleId == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID or role ID provided for role removal.");
                return IdentityResult.Failed(new IdentityError { Description = "User ID or Role ID cannot be empty." });
            }

            try
            {
                _logger.LogInformation("Removing role with ID: {RoleId} from user with ID: {UserId}", RoleId, UserId);
                var user = await _userManager.FindByIdAsync(UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found.", UserId);
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                var role = await _roleManager.FindByIdAsync(RoleId.ToString());
                if (role == null)
                {
                    _logger.LogWarning("Role with ID: {RoleId} not found.", RoleId);
                    return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
                }

                if (!await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    _logger.LogInformation("User with ID: {UserId} is not in role: {RoleName}", UserId, role.Name);
                    return IdentityResult.Success;
                }

                var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Successfully removed role with ID: {RoleId} from user with ID: {UserId}", RoleId, UserId);
                }
                else
                {
                    _logger.LogWarning("Failed to remove role with ID: {RoleId} from user with ID: {UserId}. Errors: {Errors}", RoleId, UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing role with ID: {RoleId} from user with ID: {UserId}", RoleId, UserId);
                throw new InvalidOperationException($"An error occurred while removing the role with role Id: {RoleId}.", ex);
            }
        }

        public async Task<List<User>> GetUsersWithoutRole()
        {
            try
            {
                _logger.LogInformation("Fetching users without roles.");
                var users = _userManager.Users.ToList();
                var usersWithoutRole = new List<User>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Count == 0)
                    {
                        usersWithoutRole.Add(user);
                    }
                }
                return usersWithoutRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users without roles.");
                throw new InvalidOperationException($"An error occurred while fetching users without roles", ex);
            }
        }

        public async Task<IEnumerable<(User, Role)>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all user-role mappings.");
                var userRoles = new List<(User, Role)>();

                var roles = _roleManager.Roles.ToList();
                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                    foreach (var user in usersInRole)
                    {
                        userRoles.Add((user, role));
                    }
                }

                return userRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all user-role mappings.");
                throw new InvalidOperationException($"An error occurred while fetching all user-role mappings.", ex);
            }
        }

        public async Task<PagedResult<(User, Role)>> GetPaginatedGuidesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            try
            {
                _logger.LogInformation("Fetching paginated user-role mappings for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);

                // Fetch all roles
                var roles = _roleManager.Roles.ToList();

                // Fetch all user-role mappings
                var userRoles = new List<(User, Role)>();
                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                    foreach (var user in usersInRole)
                    {
                        userRoles.Add((user, role));
                    }
                }

                // Calculate total items
                var totalItems = userRoles.Count;

                // Apply pagination
                var paginatedItems = userRoles
                    .OrderBy(ur => ur.Item1.UserName) // Sort by username
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Create the paginated result
                var result = new PagedResult<(User, Role)>
                {
                    Items = paginatedItems,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated user-role mappings.");
                throw new InvalidOperationException("An error occurred while fetching paginated user-role mappings.", ex);
            }
        }
    }
}

