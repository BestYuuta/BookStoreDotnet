using System;
using System.Collections.Generic;
using System.Linq;
using Bookstore.DTO;
using BookStoreDotnet.Config;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.DAL
{
    public class BookDAL
    {
        private BookStore _context = new BookStore();
        public List<BookDTO> GetBooks()
        {
            return _context.Books
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Stock = b.Stock,
                    BookCover = b.BookCover,
                    CreatedAt = b.CreatedAt
                }).ToList();
        }

        public List<BookDTO> GetBookByTitle(string title = "")
        {
            return _context.Books
                .Where(b => string.IsNullOrEmpty(title) || b.Title.Contains(title))
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Stock = b.Stock,
                    BookCover = b.BookCover,
                    CreatedAt = b.CreatedAt
                }).ToList();
        }
        public List<BookDTO> GetBookByAuthor(string author = "")
        {
            return _context.Books
                .Where(b => string.IsNullOrEmpty(author) || b.Author.Contains(author))
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Stock = b.Stock,
                    BookCover = b.BookCover,
                    CreatedAt = b.CreatedAt
                }).ToList();
        }

        public int AddBook(Books book)
        {
            _context.Books.Add(book);
            return _context.SaveChanges();
        }

        public int UpdateBook(Books book)
        {
            var existingBook = _context.Books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.BookCover = book.BookCover;
            existingBook.Stock = book.Stock;
            return _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
        }
        public BookDTO GetBookById(int id)
        {
        return _context.Books
            .Where(b => b.Id == id)
            .Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Stock = b.Stock,
                BookCover = b.BookCover,
                CreatedAt = b.CreatedAt
            }).FirstOrDefault();
        }
    }
}
