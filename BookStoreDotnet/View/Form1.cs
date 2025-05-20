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
using BookStoreDotnet.Config;
using BookStoreDotnet.View;

namespace BookStoreDotnet
{
    public partial class Form1 : Form
    {
        private static readonly UserBLL userBLL = new UserBLL();
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            string message;
            if (userBLL.Login(username, password, out message))
            {
                this.Hide();
                if (Session.UserRole == "admin")
                {
                    AdminDashboard adminDashboard = new AdminDashboard();
                    adminDashboard.FormClosed += (s, args) => this.Close();
                    adminDashboard.Show();
                }
                else
                {
                    UserDashboard userDashboard = new UserDashboard();
                    userDashboard.FormClosed += (s, args) => this.Close();
                    userDashboard.Show();
                }    

                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show(message);
            }
        }


        private void btnReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm register = new RegisterForm();
            register.FormClosed += (s, args) => this.Close();
            register.Show();
        }
    }
}
