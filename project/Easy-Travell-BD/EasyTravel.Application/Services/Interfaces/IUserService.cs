using EasyTravel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services.Interfaces
{
    public interface IUserService
    {

        void RegisterUser(User user);

        bool AuthenticateUser(string email, string password);
        User GetUserByEmail(string email);
    }
}
