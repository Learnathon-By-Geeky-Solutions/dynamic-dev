using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IRegisterService
    {
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(User user, string password);
    }
}
