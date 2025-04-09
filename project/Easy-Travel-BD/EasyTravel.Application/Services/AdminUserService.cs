using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAdminRoleService _adminRoleService;
        public AdminUserService(UserManager<User> userManager, IAdminRoleService adminRoleService)
        {
            _userManager = userManager;
            _adminRoleService = adminRoleService;
        }
        public async Task<(bool Success,string ErrorMessage)> CreateAsync(User user,string password, string role)
        {

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }

            await _userManager.AddToRoleAsync(user, role);
            return (true, string.Empty);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
               await _userManager.DeleteAsync(user);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _userManager.Users;
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public User GetUserInstance()
        {
            return new User();
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAsync(string firstName, string lastName, DateTime? dateOfBirth, string gender, string email, string userName, string role)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Email = email,
                UserName = email
            };

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, role);

            return (true, string.Empty);
        }

        public Task<(bool Success, string ErrorMessage)> UpdateAsync(User user, string role)
        {
            throw new NotImplementedException();
        }
    }
}
