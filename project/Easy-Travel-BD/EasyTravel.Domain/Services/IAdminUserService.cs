using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminUserService
    {
        User GetUserInstance();
        Task<(bool Success, string ErrorMessage)> CreateAsync(User user,string password, string role);
        Task<(bool Success, string ErrorMessage)> UpdateAsync(User user, string role);
        Task DeleteAsync(Guid id);
        Task<User> GetAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
    }
}
