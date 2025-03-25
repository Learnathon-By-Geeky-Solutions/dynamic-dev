using EasyTravel.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminUserRoleService
    {
        Task<IdentityResult> CreateAsync(Guid UserId, Guid RoleId);
        Task<IdentityResult> UpdateAsync(Guid UserId, Guid RoleId);
        Task<IdentityResult> DeleteAsync(Guid UserId, Guid RoleId);
        Task<IEnumerable<(User, Role)>> GetAllAsync();
    }
}
