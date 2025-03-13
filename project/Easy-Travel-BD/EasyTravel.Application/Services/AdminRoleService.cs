using EasyTravel.Domain;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class AdminRoleService : IAdminRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        public AdminRoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task CreateAsync(Role entity)
        {
            await _roleManager.CreateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }

        public async Task<Role> GetAsync(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task UpdateAsync(Role entity)
        {
            await _roleManager.UpdateAsync(entity);
        }
    }
}
