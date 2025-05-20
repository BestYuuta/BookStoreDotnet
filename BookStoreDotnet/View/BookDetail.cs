using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Bookstore.DTO;
using BookStoreDotnet.BLL;
using BookStoreDotnet.Config;

namespace BookStoreDotnet.View
{
    public partial class BookDetail : Form
    {
        private BookBLL bookBLL = new BookBLL();

        public BookDetail(int bookId)
        {
            InitializeComponent();
            LoadBookDetail(bookId);
        }

        private void LoadBookDetail(int bookId)
        {
            string role = Session.UserRole;
            btnSubmit.Text = (role == "admin") ? "Edit Book" : "Rent Book";

            var book = bookBLL.GetBookById(bookId);
            if (book != null)
            {
                txtBookId.Text = book.Id.ToString();
                txtTitle.Text = book.Title;
                txtAuthor.Text = book.Author;
                txtStock.Text = book.Stock.ToString();
                txtAddedAt.Text = book.CreatedAt.ToString("yyyy-MM-dd HH:mm");

                if (!string.IsNullOrEmpty(book.BookCover) && File.Exists(book.BookCover))
                {
                    pictureBox1.Image = Image.FromFile(book.BookCover);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
            else
            {
                MessageBox.Show("Failed to load book.");
                this.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string role = Session.UserRole;
            int bookId = Convert.ToInt32(txtBookId.Text);

            if (role == "admin")
            {
                BookEdit editForm = new BookEdit(bookId);
                editForm.FormClosed += (s, args) => LoadBookDetail(bookId);
                editForm.Show();
            }
            else
            {
                int userId = Session.UserID;
                RentalBLL rentalBLL = new RentalBLL();

                bool? isRented = rentalBLL.IsBookRentedByUser(userId, bookId);
                if (isRented == null)
                {
                    MessageBox.Show("Failed to check rental status.");
                    return;
                }

                if (isRented == true)
                {
                    MessageBox.Show("You have already rented this book and haven't returned it.");
                    return;
                }

                var rental = new Rentals
                {
                    UserId = userId,
                    BookId = bookId,
                    RentDate = DateTime.Now,
                    Status = Rentals.RentalStatus.Rented
                };

                bool rentSuccess = rentalBLL.RentBook(Session.UserID, bookId);
                if (rentSuccess)
                {
                    this.Close();
                    MessageBox.Show("Book rented successfully!");
                }
                else
                {
                    MessageBox.Show("Rent failed.");
                }
            }
        }
    }
}
