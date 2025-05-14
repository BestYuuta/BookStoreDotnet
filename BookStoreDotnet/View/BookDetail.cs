using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BookStoreDotnet.BLL;
using BookStoreDotnet.Config;
using BookStoreDotnet.DTO;

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

            var response = bookBLL.GetBookById(bookId);
            if (response.Success && response.Data is BookDTO book)
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
                MessageBox.Show("Failed to load book: " + response.Message);
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

            if (role == "admin")
            {
                int bookId = Convert.ToInt32(txtBookId.Text);
                BookEdit editForm = new BookEdit(bookId);
                editForm.FormClosed += (s, args) => LoadBookDetail(bookId);
                editForm.Show();
            }
            else
            {
                int bookId = Convert.ToInt32(txtBookId.Text);
                int userId = Session.UserID;

                RentalBLL rentalBLL = new RentalBLL();
                var checkResponse = rentalBLL.IsBookRentedByUser(userId, bookId);

                if (!checkResponse.Success)
                {
                    MessageBox.Show("Failed to check rental status: " + checkResponse.Message);
                    return;
                }

                bool alreadyRented = Convert.ToBoolean(checkResponse.Data);
                if (alreadyRented)
                {
                    MessageBox.Show("You have already rented this book and haven't returned it.");
                    return;
                }

                RentalDTO rental = new RentalDTO
                {
                    UserId = userId,
                    BookId = bookId,
                    RentDate = DateTime.Now,
                    Status = "Rented"
                };

                var rentResponse = rentalBLL.RentBook(rental);
                if (rentResponse.Success)
                {
                    this.Close();
                    MessageBox.Show("Book rented successfully!");
                }
                else
                {
                    MessageBox.Show("Rent failed: " + rentResponse.Message);
                }
            }
        }

    }
}
