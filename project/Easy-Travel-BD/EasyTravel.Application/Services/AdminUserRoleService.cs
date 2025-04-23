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
    public class AdminUserRoleService : IAdminUserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public AdminUserRoleService(UserManager<User> userManager,RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> DeleteAsync(Guid UserId, Guid RoleId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            // Check if the user is in the role
            if (!await _userManager.IsInRoleAsync(user, role.Name))
                return IdentityResult.Success; // No need to remove if not in role

            return await _userManager.RemoveFromRoleAsync(user, role.Name);
        }

        public async Task<IdentityResult> UpdateAsync(Guid UserId, Guid RoleId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            // Remove all existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
                return removeResult;

            return await CreateAsync(UserId, RoleId);
        }

        public async Task<List<User>> GetUsersWithoutRole()
        {
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
        public async Task<IdentityResult> CreateAsync(Guid UserId, Guid RoleId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            // Check if already in role to avoid duplicate assignment
            if (await _userManager.IsInRoleAsync(user, role.Name))
                return IdentityResult.Success;

            return await _userManager.AddToRoleAsync(user, role.Name);
        }



        public async Task<IEnumerable<(User, Role)>> GetAllAsync()
        {
            var userRoles = new List<(User, Role)>();
            var adminUsers = await _userManager.GetUsersInRoleAsync("admin");
            var adminRole = await _roleManager.FindByNameAsync("admin");
            foreach (var user in adminUsers)
            {
                userRoles.Add((user, adminRole));
            }
            var clientUsers = await _userManager.GetUsersInRoleAsync("client");
            var clientRole = await _roleManager.FindByNameAsync("client");
            foreach (var user in clientUsers)
            {
                userRoles.Add((user, clientRole));
            }
            var agencyManager = await _userManager.GetUsersInRoleAsync("agencyManager");
            var agencyRole = await _roleManager.FindByNameAsync("agencyManager");
            foreach (var user in agencyManager)
            {
                userRoles.Add((user, agencyRole));
            }
            var busManager = await _userManager.GetUsersInRoleAsync("busManager");
            var busRole = await _roleManager.FindByNameAsync("busManager");
            foreach (var user in busManager)
            {
                userRoles.Add((user, busRole));
            }
            var carManager = await _userManager.GetUsersInRoleAsync("carManager");
            var carRole = await _roleManager.FindByNameAsync("carManager");
            foreach (var user in carManager)
            {
                userRoles.Add((user, carRole));
            }
            var hotelManager = await _userManager.GetUsersInRoleAsync("hotelManager");
            var hotelRole = await _roleManager.FindByNameAsync("hotelManager");
            foreach (var user in hotelManager)
            {
                userRoles.Add((user, hotelRole));
            }
            return userRoles;
        }
    }
}
