using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Repositories
{
    public interface IUserRepository:IRepository<User,Guid>
    {
        User GetUserByEmail(string email);
        void AddUser(User user);
        bool ValidateUser(string email, string password);
        IEnumerable<User> GetAllUsers();


    }
}
