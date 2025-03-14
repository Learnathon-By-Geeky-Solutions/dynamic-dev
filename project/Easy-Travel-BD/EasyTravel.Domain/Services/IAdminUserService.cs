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
        Task<(bool Success, string ErrorMessage)> CreateAsync(string firstName, string lastName, DateTime? dateOfBirth, string gender, string email, string userName, string password, string role);
        Task<(bool Success, string ErrorMessage)> UpdateAsync(string firstName, string lastName, DateTime? dateOfBirth, string gender, string email, string userName, string role);
        Task DeleteAsync(Guid id);
        Task<User> GetAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
