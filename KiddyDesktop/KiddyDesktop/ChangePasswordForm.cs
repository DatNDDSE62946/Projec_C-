using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiddyDesktop.Models;

namespace KiddyDesktop
{
    public partial class ChangePasswordForm : Form
    {
        KiddyStore data = new KiddyStore();
        tblEmployee emp;
        
        public ChangePasswordForm()
        {
            InitializeComponent();
        }
        public ChangePasswordForm(string username)
        {
            InitializeComponent();
            emp = data.tblEmployees.Single(emp => emp.username.Equals(username));
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text.Equals(emp.password))
            {
                if(txtNewPassword.Text == txtConfirmPassword.Text)
                {
                    emp.password = txtNewPassword.Text;
                    data.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Confirm password is not correct");
                }
            }
            else
            {
                MessageBox.Show("Your password is not correct!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
