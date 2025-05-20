using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookstore.DTO;

namespace Bookstore.DTO
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Rentals = new HashSet<Rentals>();
            CreatedAt = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } 
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string Role { get; set; } = "user"; 
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Rentals> Rentals { get; set; }
    }
}
