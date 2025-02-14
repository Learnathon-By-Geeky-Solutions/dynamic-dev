
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

        private readonly IApplicationUnitOfWork _applicationUnitOfWork1;
        public UserService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork1 = applicationUnitOfWork;

        }


        public bool AuthenticateUser(string email, string password)
        {
            return _applicationUnitOfWork1.UserRepository.ValidateUser(email, password);
        }

        public User GetUserByEmail(string email)
        {
            return _applicationUnitOfWork1.UserRepository.GetUserByEmail(email);
        }

        public void RegisterUser(User user)
        {
            _applicationUnitOfWork1.UserRepository.AddUser(user);
            _applicationUnitOfWork1.Save();
        }
    }
}
