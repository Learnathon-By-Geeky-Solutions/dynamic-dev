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
        public AdminUserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<(bool Success,string ErrorMessage)> CreateAsync(User user,string password)
        {
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
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

        public async Task<User?> GetAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        public User GetUserInstance()
        {
            return new User();
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAsync(User user)
        {
            user.UserName = user.Email;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }

            return (true, string.Empty);
        }
    }
}
