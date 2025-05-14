using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.DTO;

namespace BookStoreDotnet.DAL
{
    public class UserDAL
    {
        private readonly BookStore _context = new BookStore();

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges();
        }

        public bool IsUsernameExist(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
