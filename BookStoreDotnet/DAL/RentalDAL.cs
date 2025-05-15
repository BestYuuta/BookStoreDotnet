using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.DTO;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.DAL
{
    public class RentalDAL
    {
        private BookStore _context = new BookStore();
        public int RentBook(RentalDTO rentalDTO)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(b => b.Id == rentalDTO.BookId);
                if (book == null || book.Stock <= 0)
                {
                    return 0;
                }
                var rental = new Rentals
                {
                    UserId = rentalDTO.UserId,
                    BookId = rentalDTO.BookId,
                    RentDate = DateTime.Now,
                    Status = Rentals.RentalStatus.Rented
                };
                _context.Rentals.Add(rental);
                book.Stock -= 1;
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public List<RentalDTO> GetRentalsByUserId(int userId)
        {
            return _context.Rentals
                .Where(r => r.UserId == userId)
                .Select(r => new RentalDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title,
                    RentDate = r.RentDate,
                    ReturnDate = r.ReturnDate,
                    Status = r.Status.ToString()
                }).ToList();
        }
        public int ReturnBook(int rentalId)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.Id == rentalId);
            if (rental != null)
            {
                rental.ReturnDate = DateTime.Now;
                rental.Status = Rentals.RentalStatus.Returned;
                var days = (rental.ReturnDate.Value - rental.RentDate).TotalDays;
                int roundedDays = (int)Math.Ceiling(days);
                rental.RentalFee = roundedDays * 3000;
                var book = _context.Books.FirstOrDefault(b => b.Id == rental.BookId);
                if (book != null)
                {
                    book.Stock += 1;
                }
                _context.SaveChanges();
                return (int)rental.RentalFee;
            }
            return 0;
        }
        public bool IsBookRentedByUser(int userId, int bookId)
        {
            return _context.Rentals.Any(r =>
                r.UserId == userId &&
                r.BookId == bookId &&
                r.Status == Rentals.RentalStatus.Rented);
        }
    }
}
