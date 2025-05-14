using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.DTO;
using BookStoreDotnet.DAL;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.BLL
{
    public class BookBLL
    {
        private static readonly BookDAL bookDAL = new BookDAL();
        public ResponseDTO GetBooks()
        {
            try
            {
                return new ResponseDTO
                {
                    Success = true,
                    Data = bookDAL.GetBooks(),
                    Message = "Books retrieved successfully"
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
        public ResponseDTO GetBookByTitle(string title)
        {
            try
            {
                return new ResponseDTO
                {
                    Success = true,
                    Data = bookDAL.GetBookByTitle(title),
                    Message = "Books retrieved successfully"
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
        public ResponseDTO GetBookByAuthor(string author)
        {
            try
            {
                return new ResponseDTO
                {
                    Success = true,
                    Data = bookDAL.GetBookByAuthor(author),
                    Message = "Books retrieved successfully"
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
        public ResponseDTO AddBook(Books book)
        {
            try
            {
                if (bookDAL.AddBook(book) > 0)
                {
                    return new ResponseDTO
                    {
                        Success = true,
                        Data = book,
                        Message = "Book added successfully"
                    };
                }
                else
                {
                    throw new Exception("Failed to add book");
                }
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
        public ResponseDTO UpdateBook(Books book)
        {
            try
            {
                int result = bookDAL.UpdateBook(book);
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        Success = true,
                        Data = book,
                        Message = "Book updated successfully"
                    };
                }
                else
                {
                    throw new Exception("Failed to update book");
                }
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
        public ResponseDTO DeleteBook(int id)
        {
            try
            {
                bookDAL.DeleteBook(id);
                return new ResponseDTO
                {
                    Success = true,
                    Data = null,
                    Message = "Book deleted successfully"
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
        public ResponseDTO GetBookById(int id)
        {
            try
            {
                var book = bookDAL.GetBookById(id);
                if (book != null)
                {
                    return new ResponseDTO
                    {
                        Success = true,
                        Data = book,
                        Message = "Book retrieved successfully"
                    };
                }
                else
                {
                    throw new Exception("Book not found");
                }
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
    }
}
