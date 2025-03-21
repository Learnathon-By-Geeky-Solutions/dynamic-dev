using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface ILoginService
    {
        Task<(bool Success, string ErrorMessage, string? RedirectUrl)> AuthenticateUserAsync(string email, string password, bool rememberMe);
    }
}
