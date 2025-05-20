using System;
using System.Linq;
using BookStoreDotnet.Config;
using Bookstore.DTO;

namespace BookStoreDotnet.BLL
{
    public class UserBLL
    {
        private readonly BookStore _context = new BookStore();

        public bool Register(string name, string username, string password, out string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new Exception("Name cannot be empty");

                if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
                    throw new Exception("Username must be at least 4 characters");

                if (username.Any(char.IsWhiteSpace))
                    throw new Exception("Username cannot contain spaces");

                if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
                    throw new Exception("Password must be at least 4 characters");

                if (_context.Users.Any(u => u.Username == username))
                    throw new Exception("Username already exists");

                var user = new User
                {
                    Name = name,
                    Username = username,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                    Role = "user",
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                message = "Registration successful";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool Login(string username, string password, out string message)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                    throw new Exception("Wrong username or password");

                Session.SetSession(user.Id, user.Name, user.Role);

                message = "Login successful";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
