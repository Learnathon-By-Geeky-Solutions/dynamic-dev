
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
<<<<<<< HEAD
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public UserService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
=======

        private readonly IApplicationUnitOfWork _applicationUnitOfWork1;
        public UserService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork1 = applicationUnitOfWork;

>>>>>>> 8b0483b (created a partialbusfrom for bus create,update, delete)
        }

    
        public bool AuthenticateUser(string email, string password)
        {
<<<<<<< HEAD
            return _applicationUnitOfWork.UserRepository.ValidateUser(email, password);
=======
            return _applicationUnitOfWork1.UserRepository.ValidateUser(email, password);
>>>>>>> 8b0483b (created a partialbusfrom for bus create,update, delete)
        }

        public User GetUserByEmail(string email)
        {
<<<<<<< HEAD
            return _applicationUnitOfWork.UserRepository.GetUserByEmail(email);
        }

        public string GetUserController(string role)
        {
            return role == "Admin" 
                ? "Dashboard" 
                : "Home";
        }

        public bool IsAdmin(string role)
        {
            return role == "Admin";
        }

        public bool IsLoggedIn(string status)
        {
            return status == "true";
=======
            return _applicationUnitOfWork1.UserRepository.GetUserByEmail(email);
>>>>>>> 8b0483b (created a partialbusfrom for bus create,update, delete)
        }

        public void RegisterUser(User user)
        {
<<<<<<< HEAD
            _applicationUnitOfWork.UserRepository.AddUser(user);
            _applicationUnitOfWork.Save();
=======
            _applicationUnitOfWork1.UserRepository.AddUser(user);
            _applicationUnitOfWork1.Save();
>>>>>>> 8b0483b (created a partialbusfrom for bus create,update, delete)
        }
    }
}
