using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiddyDesktop.Models;
using Newtonsoft.Json;

namespace KiddyDesktop
{
    public partial class frmLogin : Form
    {
        private KiddyStore data = new KiddyStore();
        private static HttpClient client = new HttpClient();
        private static readonly string BASE_URL = "http://localhost:50815/api/Employees/";
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
        private async void Login(EmployeeDTO dto)
        {
            EmployeeDTO result = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(BASE_URL + "CheckLogin", dto);
            string strResponse = response.Content.ReadAsStringAsync().Result;
            result = JsonConvert.DeserializeObject<EmployeeDTO>(strResponse);
            if(result == null)
            {
                MessageBox.Show("Invalid username password!");
            }
            else
            {
                Form f = new frmMain(dto);
                        f.Show();
                       this.Hide();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            EmployeeDTO dto = new EmployeeDTO
            {
                username = txtUsername.Text,
                password = txtPassword.Text
            };
            Login(dto);

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
