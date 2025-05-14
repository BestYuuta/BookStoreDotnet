using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookStoreDotnet.BLL;
using BookStoreDotnet.DTO;

namespace BookStoreDotnet.View
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            UserBLL userBLL = new UserBLL();
            ResponseDTO response = userBLL.Register(name, username, password);

            if (response.Success)
            {
                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
                MessageBox.Show("Register successful!");
            }
            else
            {
                MessageBox.Show(response.Message);
            }
        }
    }
}
