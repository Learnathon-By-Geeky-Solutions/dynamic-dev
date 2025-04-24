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
    public class AdminRoleService : IAdminRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AdminRoleService> _logger;

        public AdminRoleService(RoleManager<Role> roleManager, ILogger<AdminRoleService> logger)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CreateAsync(Role entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to create a null role entity.");
                throw new ArgumentNullException(nameof(entity), "Role entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new role with name: {Name}", entity.Name);
                var result = await _roleManager.CreateAsync(entity);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to create role: {Name}. Errors: {Errors}", entity.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new InvalidOperationException($"Failed to create role: {entity.Name}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                _logger.LogInformation("Successfully created role with name: {Name}", entity.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the role with name: {Name}", entity.Name);
                throw new InvalidOperationException($"An error occurred while creating the role with name: {entity.Name}.", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid role ID provided for deletion.");
                throw new ArgumentException("Role ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete role with ID: {Id}", id);
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    _logger.LogWarning("Role with ID: {Id} not found for deletion.", id);
                    throw new KeyNotFoundException($"Role with ID: {id} not found.");
                }

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to delete role with ID: {Id}. Errors: {Errors}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new InvalidOperationException($"Failed to delete role with ID: {id}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                _logger.LogInformation("Successfully deleted role with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the role with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the role with ID: {id}.", ex);
            }
        }

        public async Task<Role> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid role ID provided for retrieval.");
                throw new ArgumentException("Role ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching role with ID: {Id}", id);
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    _logger.LogWarning("Role with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Role with ID: {id} not found.");
                }
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the role with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the role with ID: {id}.", ex);
            }
        }

        public IEnumerable<Role> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all roles.");
                return _roleManager.Roles.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all roles.");
                throw new InvalidOperationException("An error occurred while fetching all roles.", ex);
            }
        }

        public async Task UpdateAsync(Role entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to update a null role entity.");
                throw new ArgumentNullException(nameof(entity), "Role entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating role with ID: {Id}", entity.Id);
                var result = await _roleManager.UpdateAsync(entity);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to update role with ID: {Id}. Errors: {Errors}", entity.Id, string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new InvalidOperationException($"Failed to update role with ID: {entity.Id}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                _logger.LogInformation("Successfully updated role with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the role with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while updating the role with ID: {entity.Id}.", ex);
            }
        }
    }
}

