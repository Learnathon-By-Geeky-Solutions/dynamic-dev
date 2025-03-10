using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(string firstName, string lastName, DateTime dateOfBirth, string gender, string email, string userName, string password);
        Task<(bool Success, string ErrorMessage, string? RedirectUrl)> AuthenticateUserAsync(string email, string password, bool rememberMe);
    }
}
