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
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<User> _userManager;
        public RegisterService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errorMessage);
            }
            await _userManager.AddToRoleAsync(user, "client");
            return (true, string.Empty);
        }
    }
}
