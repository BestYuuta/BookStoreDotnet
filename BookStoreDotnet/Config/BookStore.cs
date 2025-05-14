using System;
using System.Data.Entity;
using System.Linq;
using Bookstore.DTO;
using BookStoreDotnet.Config;

namespace BookStoreDotnet
{
    public class BookStore : DbContext
    {
        public BookStore()
            : base("name=BookStore")
        {
            Database.SetInitializer<BookStore>(new DBCreate());
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Rentals> Rentals { get; set; }
    }
}