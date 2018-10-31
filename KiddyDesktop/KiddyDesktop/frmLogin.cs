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
    public partial class frmLogin : Form
    {
        private KiddyStore data = new KiddyStore();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tblEmployee employee = data.tblEmployees.Single(em => em.username.Equals(txtUsername.Text));
                if (employee.password == txtPassword.Text)
                {
                    Form f = new frmMain(employee.username);
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username password!");
                    
                }
             }
            catch (Exception)
            {
               
                MessageBox.Show("Invalid username password!");
            }

}

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
