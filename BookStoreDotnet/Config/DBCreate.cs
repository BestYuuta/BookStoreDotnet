using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.DTO;

namespace BookStoreDotnet.Config
{
    class DBCreate :
        CreateDatabaseIfNotExists<BookStore>
    {
        protected override void Seed(BookStore context)
        {
            context.Users.Add(new User
            {
                Id = 1,
                Name = "Admin",
                Username = "admin",
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                Role = "admin"
            });
            context.SaveChanges();
        }
    }
}
