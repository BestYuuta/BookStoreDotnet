using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.DTO;
using BookStoreDotnet.Config;
using BookStoreDotnet.DAL;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.BLL
{
    public class UserBLL
    {
        private static readonly UserDAL userDAL = new UserDAL();

        public ResponseDTO Register(string name, string username, string password)
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

                if (userDAL.IsUsernameExist(username))
                    throw new Exception("Username already exists");

                var user = new User
                {
                    Name = name,
                    Username = username,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                    Role = "user", 
                    CreatedAt = DateTime.Now
                };

                int result = userDAL.AddUser(user);

                if (result == 0)
                    throw new Exception("Failed to register user");

                return new ResponseDTO
                {
                    Success = true,
                    Data = null,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        public ResponseDTO Login(string username, string password)
        {
            try
            {
                var user = userDAL.GetUserByUsername(username);
                if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                    throw new Exception("Wrong username or password");

                Session.SetSession(user.Id, user.Name, user.Role);
                return new ResponseDTO { Success = true, Data = null, Message = "Login successful" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Success = false, Data = null, Message = ex.Message };
            }
        }
        public ResponseDTO GetUserById(int id)
        {
            try
            {
                var user = userDAL.GetUserById(id);
                return new ResponseDTO { Success = true, Data = user, Message = "User fetched successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Success = false, Data = null, Message = ex.Message };
            }
        }
    }
}
