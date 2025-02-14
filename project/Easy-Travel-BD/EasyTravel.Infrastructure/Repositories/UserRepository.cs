
using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTravel.Domain.Repositories;

namespace EasyTravel.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, Guid>,IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
            :base(context)
        {
            _context = context;
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
     

        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

          
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with email {email} was not found.");
                }
            

            return user;
        }

        public bool ValidateUser(string email, string password)
        {
            return _context.Users.Any(u => u.Email == email && u.Password == password);
        }
    }
}
