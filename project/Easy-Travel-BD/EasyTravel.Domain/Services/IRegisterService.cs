using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IRegisterService
    {
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(string firstName, string lastName, DateTime dateOfBirth, string gender, string email, string userName, string password);
    }
}
