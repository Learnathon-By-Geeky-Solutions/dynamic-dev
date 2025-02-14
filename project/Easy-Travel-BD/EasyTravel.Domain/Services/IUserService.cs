using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IUserService
    {
        void RegisterUser(User user);
        bool AuthenticateUser(string email, string password);
        User GetUserByEmail(string email);
        bool IsLoggedIn(string status);
        string GetUserController(string role);
        bool IsAdmin(string role);
    }
}
