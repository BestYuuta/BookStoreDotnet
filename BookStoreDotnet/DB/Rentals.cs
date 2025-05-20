using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.DTO;

namespace Bookstore.DTO
{
    [Table("Rentals")]
    public class Rentals
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Books Book { get; set; }
        [Required]
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public enum RentalStatus
        {
            Rented,
            Returned
        }
        [Required]
        public RentalStatus Status { get; set; } = RentalStatus.Rented;
        [Required]
        public decimal RentalFee { get; set; }
    }
}
