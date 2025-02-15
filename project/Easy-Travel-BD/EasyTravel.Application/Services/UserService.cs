using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public UserService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public bool AuthenticateUser(string email, string password)
        {
            return _applicationUnitOfWork.UserRepository.ValidateUser(email, password);
        }

        public User GetUserByEmail(string email)
        {
            return _applicationUnitOfWork.UserRepository.GetUserByEmail(email);
        }

        public string GetUserController(string role)
        {
            return role == "Admin" ? "Dashboard" : "Home";
        }

        public bool IsAdmin(string role)
        {
            return role == "Admin";
        }

        public bool IsLoggedIn(string status)
        {
            return status == "true";
        }

        public void RegisterUser(User user)
        {
            _applicationUnitOfWork.UserRepository.AddUser(user);
            _applicationUnitOfWork.Save();
        }
    }
}
