using System;
using System.Linq;
using System.Windows.Forms;
using BookStoreDotnet.BLL;
using BookStoreDotnet.Config;

namespace BookStoreDotnet.View
{
    public partial class UserDashboard : Form
    {
        private static readonly UserBLL userBLL = new UserBLL();
        private static readonly BookBLL bookBLL = new BookBLL();
        private readonly RentalBLL rentalBLL = new RentalBLL();
        private bool showingRentals = false;

        public UserDashboard()
        {
            InitializeComponent();
            cbBFind.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Session.ClearSession();
            this.Hide();
            var login = new Form1();
            login.FormClosed += (s, args) => this.Close();
            login.Show();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string selectedOption = cbBFind.SelectedItem?.ToString();
            string keyword = txtFind.Text.Trim();

            if (string.IsNullOrEmpty(selectedOption))
            {
                MessageBox.Show("Please select a search option (Title or Author).");
                return;
            }

            var books = keyword == string.Empty
                ? bookBLL.GetBooks()
                : selectedOption == "Find by Title"
                    ? bookBLL.GetBookByTitle(keyword)
                    : selectedOption == "Find by Author"
                        ? bookBLL.GetBookByAuthor(keyword)
                        : null;

            if (books != null)
            {
                showingRentals = false;
                dataGridView1.DataSource = books;
                if (dataGridView1.Columns.Contains("BookCover"))
                    dataGridView1.Columns["BookCover"].Visible = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                MessageBox.Show("Search failed.");
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (showingRentals)
                {
                    MessageBox.Show("Cannot view book details from rentals list.");
                    return;
                }

                int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                BookDetail bookDetail = new BookDetail(bookId);
                bookDetail.Show();
            }
            catch
            {
                MessageBox.Show("Please select a book to view details.");
            }
        }

        private void btnMyRent_Click(object sender, EventArgs e)
        {
            LoadMyRentals();
        }

        private void btnRent_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to rent.");
                return;
            }

            if (showingRentals)
            {
                MessageBox.Show("Please search books to rent, not from rental list.");
                return;
            }

            int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            int userId = Session.UserID;

            bool? isAlreadyRented = rentalBLL.IsBookRentedByUser(userId, bookId);
            if (isAlreadyRented == null)
            {
                MessageBox.Show("Failed to check rental status.");
                return;
            }
            if (isAlreadyRented == true)
            {
                MessageBox.Show("You have already rented this book and haven't returned it yet.");
                return;
            }

            bool success = rentalBLL.RentBook(userId, bookId);
            if (success)
            {
                MessageBox.Show("Book rented successfully!");
            }
            else
            {
                MessageBox.Show("Failed to rent book.");
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!showingRentals)
            {
                MessageBox.Show("Please switch to your rentals to return books.");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a rental to return.");
                return;
            }

            int rentalId;
            try
            {
                rentalId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            }
            catch
            {
                MessageBox.Show("Invalid selection.");
                return;
            }

            decimal? rentalFee = rentalBLL.ReturnBook(rentalId);

            if (rentalFee.HasValue)
            {
                LoadMyRentals();
                MessageBox.Show($"Book returned successfully. Rental fee: {rentalFee.Value} VND");
            }
            else
            {
                MessageBox.Show("Failed to return book.");
            }
        }

        private void LoadMyRentals()
        {
            int userId = Session.UserID;
            var rentals = rentalBLL.GetRentalsByUserId(userId);

            if (rentals != null)
            {
                showingRentals = true;
                var rentalDisplay = rentals.Select(r => new
                {
                    r.Id,
                    BookTitle = r.Book?.Title,
                    r.RentDate,
                    r.ReturnDate,
                    r.Status,
                    r.RentalFee
                }).ToList();

                dataGridView1.DataSource = rentalDisplay;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                MessageBox.Show("Failed to load rentals.");
            }
        }

    }
}
