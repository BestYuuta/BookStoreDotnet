using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDotnet.DAL;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.BLL
{
    public class RentalBLL
    {
        private static readonly RentalDAL rentalDAL = new RentalDAL();
        public ResponseDTO RentBook(RentalDTO rentalDTO)
        {
            try
            {
                int result = rentalDAL.RentBook(rentalDTO);
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        Success = true,
                        Data = null,
                        Message = "Book rented successfully"
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        Success = false,
                        Data = null,
                        Message = "Failed to rent book"
                    };
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
        public ResponseDTO GetRentalsByUserId(int userId)
        {
            try
            {
                var rentals = rentalDAL.GetRentalsByUserId(userId);
                return new ResponseDTO
                {
                    Success = true,
                    Data = rentals,
                    Message = "Rentals retrieved successfully"
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
        public ResponseDTO ReturnBook(int rentalId)
        {
            try
            {
                decimal? rentalFee = rentalDAL.ReturnBook(rentalId);
                int result = rentalDAL.ReturnBook(rentalId);
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        Success = true,
                        Data = rentalFee,
                        Message = "Book returned successfully"
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        Success = false,
                        Data = null,
                        Message = "Failed to return book"
                    };
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
        public ResponseDTO IsBookRentedByUser(int userId, int bookId)
        {
            try
            {
                bool isRented = rentalDAL.IsBookRentedByUser(userId, bookId);
                return new ResponseDTO
                {
                    Success = true,
                    Data = isRented,
                    Message = "Check completed successfully"
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
    }
}
