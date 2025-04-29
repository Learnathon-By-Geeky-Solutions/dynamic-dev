using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
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
        Task<(bool Success, string ErrorMessage)> CreateAsync(User user,string password);
        Task<(bool Success, string ErrorMessage)> UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<User?> GetAsync(Guid id);
        IEnumerable<User> GetAll();
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsExist(string email);
        Task<PagedResult<User>> GetPaginatedUsersAsync(int pageNumber, int pageSize);
    }
}
