using System;
using System.Collections.Generic;
using System.Linq;
using Bookstore.DTO;
using BookStoreDotnet.Config; 

namespace BookStoreDotnet.BLL
{
    public class BookBLL
    {
        private readonly BookStore _context;

        public BookBLL()
        {
            _context = new BookStore();
        }

        public List<Books> GetBooks()
        {
            return _context.Books.ToList();
        }

        public List<Books> GetBookByTitle(string title)
        {
            return _context.Books
                .Where(b => string.IsNullOrEmpty(title) || b.Title.Contains(title))
                .ToList();
        }

        public List<Books> GetBookByAuthor(string author)
        {
            return _context.Books
                .Where(b => string.IsNullOrEmpty(author) || b.Author.Contains(author))
                .ToList();
        }

        public Books GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }

        public bool AddBook(Books book)
        {
            _context.Books.Add(book);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateBook(Books book)
        {
            var existing = _context.Books.FirstOrDefault(b => b.Id == book.Id);
            if (existing == null) return false;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Stock = book.Stock;
            existing.BookCover = book.BookCover;

            return _context.SaveChanges() > 0;
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;

            _context.Books.Remove(book);
            return _context.SaveChanges() > 0;
        }
    }
}
