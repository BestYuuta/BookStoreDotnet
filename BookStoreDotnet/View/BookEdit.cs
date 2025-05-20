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

namespace BookStoreDotnet.View
{
    public partial class BookEdit : Form
    {
        private string selectedImagePath = string.Empty;
        private int? bookId = null;
        private BookBLL bookBLL = new BookBLL();
        public BookEdit(int bookId)
        {
            this.bookId = bookId;
            InitializeComponent();
            if (bookId != -1)
            {
                LoadBookData(bookId);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Choose Book Cover";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;

                    pictureBox1.Image = Image.FromFile(selectedImagePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string stockText = txtStock.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(stockText))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(stockText, out int stock))
            {
                MessageBox.Show("Stock must be a valid number.");
                return;
            }
            if (stock < 1)
            {
                MessageBox.Show("Stock must be greater than or equal to 1.");
                return;
            }

            var book = new Books
            {
                Title = title,
                Author = author,
                Stock = stock,
                BookCover = selectedImagePath
            };

            bool success;
            if (bookId == -1)
            {
                success = bookBLL.AddBook(book);
            }
            else
            {
                book.Id = bookId.Value;
                success = bookBLL.UpdateBook(book);
            }

            if (success)
            {
                MessageBox.Show(bookId == -1 ? "Book added successfully!" : "Book updated successfully!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Operation failed.");
            }
        }
        private void LoadBookData(int bookId)
        {
            var book = bookBLL.GetBookById(bookId);
            if (book != null)
            {
                txtTitle.Text = book.Title;
                txtAuthor.Text = book.Author;
                txtStock.Text = book.Stock.ToString();
                selectedImagePath = book.BookCover;

                if (!string.IsNullOrEmpty(book.BookCover) && System.IO.File.Exists(book.BookCover))
                {
                    pictureBox1.Image = Image.FromFile(book.BookCover);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            else
            {
                MessageBox.Show("Failed to load book.");
                this.Close();
            }
        }
    }
}
