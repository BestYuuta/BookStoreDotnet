using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Bookstore.DTO;
using BookStoreDotnet.Config;

namespace BookStoreDotnet.BLL
{
    public class RentalBLL
    {
        private readonly BookStore _context;

        public RentalBLL()
        {
            _context = new BookStore();
        }

        public bool RentBook(int userId, int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null || book.Stock <= 0)
            {
                return false;
            }

            var rental = new Rentals
            {
                UserId = userId,
                BookId = bookId,
                RentDate = DateTime.Now,
                Status = Rentals.RentalStatus.Rented
            };

            _context.Rentals.Add(rental);
            book.Stock -= 1;
            return _context.SaveChanges() > 0;
        }

        public List<Rentals> GetRentalsByUserId(int userId)
        {
            return _context.Rentals
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)
                .ToList();
        }

        public decimal? ReturnBook(int rentalId)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.Id == rentalId);
            if (rental == null || rental.Status == Rentals.RentalStatus.Returned)
            {
                return null;
            }

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
            return rental.RentalFee;
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
