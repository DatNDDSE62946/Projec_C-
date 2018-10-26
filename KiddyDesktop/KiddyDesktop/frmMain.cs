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
    public partial class frmMain : Form
    {
        private KiddyStore data = new KiddyStore();
        private string status;
        public frmMain()
        {
            InitializeComponent();
        }
        public frmMain(string username)
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            unLockedText();
            setBlank();
            status = "add";
        }
        private void lockedText()
        {
            txtUsername.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtPassword.Enabled = false;
            dtDOB.Enabled = false;
            rdMale.Enabled = false;
            rdMale.Enabled = false;

            btnEmployeeAdd.Enabled = true;
            btnEmployeeSave.Enabled = false;
            btnEmployeeDelete.Enabled = true;
            btnEmployeeEdit.Enabled = true;
            btnUploadImage.Enabled = false;
        }
        private void unLockedText()
        {
            txtUsername.Enabled = true;
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtPassword.Enabled = true;
            dtDOB.Enabled = true;
            rdMale.Enabled = true;
            rdMale.Enabled = true;

            btnEmployeeAdd.Enabled = false;
            btnEmployeeSave.Enabled = true;
            btnEmployeeDelete.Enabled = false;
            btnEmployeeEdit.Enabled = false;
            btnUploadImage.Enabled = true;
        }
        private void setBlank()
        {
            txtUsername.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPassword.Text = "";
        }
        private void showEmployeeData()
        {
            gvEmployee.DataSource = data.tblEmployees.Select(em => new
            {
                username = em.username,
                dob = em.dob,
                gender = em.gender,
                role = em.role,
                name = em.firstname + " " + em.lastname,
            }).Where(em => em.role.Equals("Employee")).ToList();
            gvEmployee.Columns[0].HeaderText = "Username";
            gvEmployee.Columns[1].HeaderText = "Date Of Birth";
            gvEmployee.Columns[2].HeaderText = "Gender";
            gvEmployee.Columns[3].HeaderText = "Role";
            gvEmployee.Columns[4].HeaderText = "Name";

        }
        private void viewEmployeeDetail()
        {
            string username = gvEmployee.CurrentRow.Cells[0].Value.ToString();
            tblEmployee cellEmployee = data.tblEmployees.Single(emp => emp.username.Equals(username));
            txtUsername.Text = cellEmployee.username;
            txtPassword.Text = cellEmployee.password;
            txtFirstName.Text = cellEmployee.firstname;
            txtLastName.Text = cellEmployee.lastname;
            dtDOB.Text = cellEmployee.dob;
            if (cellEmployee.gender.Equals("male"))
            {
                rdMale.Select();
            }
            else
            {
                rdFemail.Select();
            }
        }
        private void SaveEmployee()
        {
            if (!checkBlank())
            {

                try
                {
                    tblEmployee emp = data.tblEmployees.Single(em => em.username.Equals(txtUsername.Text));
                    if (emp != null)
                    {
                        MessageBox.Show("Username has already existed!");
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime myDate = DateTime.Parse(dtDOB.Text);
                        string mydob = string.Format("{0}-{1}-{2}", myDate.Year, myDate.Month, myDate.Day);
                        string myGender = "female";
                        if (rdMale.Checked == true)
                        {
                            myGender = "male";
                        }
                        tblEmployee addEmp = new tblEmployee();
                        addEmp.username = txtUsername.Text;
                        addEmp.password = txtPassword.Text;
                        addEmp.dob = mydob;
                        addEmp.gender = myGender;
                        addEmp.role = "Employee";
                        addEmp.isActived = true;
                        addEmp.firstname = txtFirstName.Text;
                        addEmp.lastname = txtLastName.Text;
                        data.tblEmployees.Add(addEmp);
                        data.SaveChanges();
                        showEmployeeData();
                        MessageBox.Show("Add employee success!");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show((new Exception()).ToString());
                    }
                }
            }
        }
        private bool checkBlank()
        {
            bool result = false;
            if (txtUsername.Text.Equals(""))
            {
                txtUsername.Focus();
                MessageBox.Show("Username get empty!");
                result = true;
            }
            if (txtPassword.Text.Equals(""))
            {
                txtUsername.Focus();
                MessageBox.Show("Password get empty!");
                result = true;
            }
            if (txtFirstName.Text.Equals(""))
            {
                MessageBox.Show("Firstname get empty!");
                txtUsername.Focus();
                result = true;
            }
            if (txtLastName.Text.Equals(""))
            {
                MessageBox.Show("Lastname get empty!");
                txtUsername.Focus();
                result = true;
            }
            return result;
        }
        private void EditEmployee()
        {
            if (!checkBlank())
            {
                try
                {
                    DateTime myDate = DateTime.Parse(dtDOB.Text);
                    string mydob = string.Format("{0}-{1}-{2}", myDate.Year, myDate.Month, myDate.Day);
                    string myGender = "female";
                    if (rdMale.Checked == true)
                    {
                        myGender = "male";
                    }
                        tblEmployee editEmp = data.tblEmployees.Single(emp => emp.username.Equals(txtUsername.Text));
                    editEmp.password = txtPassword.Text;
                    editEmp.firstname = txtFirstName.Text;
                    editEmp.lastname = txtLastName.Text;
                    editEmp.dob = mydob;
                    editEmp.gender = myGender;
                    data.SaveChanges();
                    showEmployeeData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Edit fail!");
                }
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            viewEmployeeDetail();
            lockedText();

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            showEmployeeData();
            dtDOB.Format = DateTimePickerFormat.Custom;
            dtDOB.CustomFormat = "yyyy-MMM-dd";
            rdMale.Checked = true;
        }
       
        
       
        private void btnEmployeeSave_Click(object sender, EventArgs e)
        {
            if (status.Equals("add"))
            {
                SaveEmployee();
            }
            else
            {
                EditEmployee();
            }
            
        }

        private void btnEmployeeEdit_Click(object sender, EventArgs e)
        {
            unLockedText();
            txtUsername.Enabled = false;
            btnEmployeeAdd.Enabled = false;
            btnEmployeeDelete.Enabled = false;
            status = "edit";

        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            try
            {
                tblEmployee delEmp = data.tblEmployees.Single(emp => emp.username.Equals(username));
                data.tblEmployees.Remove(delEmp);
                data.SaveChanges();
                showEmployeeData();
            }
            catch (Exception)
            {
                MessageBox.Show("Username is not existed!");
            }
        }
    }
    
}
