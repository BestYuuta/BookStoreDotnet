using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Bookstore.DTO;
using BookStoreDotnet.BLL;
using BookStoreDotnet.Config;

namespace BookStoreDotnet.View
{
    public partial class AdminDashboard : Form
    {
        private static readonly BookBLL bookBLL = new BookBLL();

        public AdminDashboard()
        {
            InitializeComponent();
            cbBFind.SelectedIndex = 0;
            LoadAllBooks();
        }
        private void btnQuit_Click(object sender, EventArgs e)
        {
            Session.ClearSession();
            this.Hide();
            var login = new Form1();
            login.FormClosed += (s, args) => this.Close();
            login.Show();
            MessageBox.Show("Logout successful");
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            string selectedOption = cbBFind.SelectedItem?.ToString();
            string keyword = txtFind.Text.Trim();

            List<Books> books = new List<Books>();

            if (string.IsNullOrEmpty(selectedOption))
            {
                MessageBox.Show("Please select a search option (Title or Author).");
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    books = bookBLL.GetBooks();
                }
                else if (selectedOption == "Find by Title")
                {
                    books = bookBLL.GetBookByTitle(keyword);
                }
                else if (selectedOption == "Find by Author")
                {
                    books = bookBLL.GetBookByAuthor(keyword);
                }
                else
                {
                    MessageBox.Show("Invalid search option.");
                    return;
                }
                LoadBooksToGrid(books);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BookEdit bookEdit = new BookEdit(-1);
            bookEdit.FormClosed += (s, args) => LoadAllBooks();
            bookEdit.Show();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                BookDetail bookDetail = new BookDetail(bookId);
                bookDetail.Show();
            }
            catch
            {
                MessageBox.Show("Please select a book to view details.");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                bool success = bookBLL.DeleteBook(bookId);
                if (success)
                {
                    MessageBox.Show("Book deleted successfully");
                    LoadAllBooks();
                }
                else
                {
                    MessageBox.Show("Failed to delete book");
                }
            }
            catch
            {
                MessageBox.Show("Please choose a book to delete.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                BookEdit bookEdit = new BookEdit(bookId);
                bookEdit.FormClosed += (s, args) => LoadAllBooks();
                bookEdit.Show();
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }

        private void LoadAllBooks()
        {
            try
            {
                var books = bookBLL.GetBooks();
                LoadBooksToGrid(books);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load books: " + ex.Message);
            }
        }

        private void LoadBooksToGrid(List<Books> books)
        {
            dataGridView1.DataSource = books;
            if (dataGridView1.Columns["BookCover"] != null)
            {
                dataGridView1.Columns["BookCover"].Visible = false;
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
