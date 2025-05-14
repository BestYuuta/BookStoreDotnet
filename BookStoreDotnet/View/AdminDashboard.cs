using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bookstore.DTO;
using BookStoreDotnet.BLL;
using BookStoreDotnet.Config;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.View
{
    public partial class AdminDashboard : Form
    {
        private static readonly BookBLL bookBLL = new BookBLL();
        public AdminDashboard()
        {
            InitializeComponent();
            cbBFind.SelectedIndex = 0;
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
            if (selectedOption == "Find by Title")
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
                dataGridView1.DataSource = response.Data;
                dataGridView1.Columns["BookCover"].Visible = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }
            else
            {
                MessageBox.Show("Search failed");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BookEdit bookEdit = new BookEdit(-1);
            bookEdit.FormClosed += (s, args) => btnFind_Click(sender, e);
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
                ResponseDTO response = bookBLL.DeleteBook(bookId);
                if (response.Success)
                {
                    MessageBox.Show("Book deleted successfully");
                    btnFind_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Failed to delete book");
                }
            }
            catch (Exception ex)
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
                bookEdit.FormClosed += (s, args) => btnFind_Click(sender, e); 
                bookEdit.Show();
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }
    }
}
