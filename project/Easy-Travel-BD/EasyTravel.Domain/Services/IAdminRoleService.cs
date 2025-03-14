using EasyTravel.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminRoleService
    {
        Task CreateAsync(Role entity);
        Task UpdateAsync(Role entity);
        Task DeleteAsync(Guid id);
        Task<Role> GetAsync(Guid id);
        Task<IEnumerable<Role>> GetAllAsync();
    }
}
