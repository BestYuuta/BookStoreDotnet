using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bookstore.DTO;

namespace Bookstore.DTO
{
    public class Books
    {
        public Books()
        {
            Rentals = new HashSet<Rentals>();
            CreatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Author { get; set; }
        [Required]
        public int Stock { get; set; }
        public string BookCover { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Rentals> Rentals { get; set; }
    }
}