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
            string message;

            UserBLL userBLL = new UserBLL();
            if (userBLL.Register(name,username, password, out message))
            {
                MessageBox.Show(message);
                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
            else
            {
                MessageBox.Show(message);
            }
        }
    }
}
