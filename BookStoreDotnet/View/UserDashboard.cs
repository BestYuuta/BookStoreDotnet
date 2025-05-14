using System;
using System.Windows.Forms;
using BookStoreDotnet.BLL;
using BookStoreDotnet.Config;
using BookStoreDotnet.DTO;

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

            ResponseDTO response;

            if (string.IsNullOrEmpty(keyword))
            {
                response = bookBLL.GetBooks();
            }
            else if (selectedOption == "Find by Title")
            {
                response = bookBLL.GetBookByTitle(keyword);
            }
            else if (selectedOption == "Find by Author")
            {
                response = bookBLL.GetBookByAuthor(keyword);
            }
            else
            {
                MessageBox.Show("Invalid search option.");
                return;
            }

            if (response.Success)
            {
                showingRentals = false;
                dataGridView1.DataSource = response.Data;
                dataGridView1.Columns["BookCover"].Visible = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                MessageBox.Show("Search failed: " + response.Message);
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

            var checkResponse = rentalBLL.IsBookRentedByUser(userId, bookId);
            if (!checkResponse.Success)
            {
                MessageBox.Show("Failed to check rental status: " + checkResponse.Message);
                return;
            }

            bool isAlreadyRented = Convert.ToBoolean(checkResponse.Data);
            if (isAlreadyRented)
            {
                MessageBox.Show("You have already rented this book and haven't returned it yet.");
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
                MessageBox.Show("Book rented successfully!");
            }
            else
            {
                MessageBox.Show("Rent failed: " + rentResponse.Message);
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

            var response = rentalBLL.ReturnBook(rentalId);

            if (response.Success)
            {
                LoadMyRentals();
                MessageBox.Show("Book returned successfully.");
            }
            else
            {
                MessageBox.Show("Failed to return book: " + response.Message);
            }
        }

        private void LoadMyRentals()
        {
            int userId = Session.UserID;
            var response = rentalBLL.GetRentalsByUserId(userId);

            if (response.Success)
            {
                showingRentals = true;
                dataGridView1.DataSource = response.Data;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                MessageBox.Show("Failed to load rentals: " + response.Message);
            }
        }
    }
}
