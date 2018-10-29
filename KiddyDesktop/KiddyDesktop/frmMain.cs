using System;
using System.Collections;
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
        tblEmployee emp;
        int ordIDConfirm;
        private KiddyStore data = new KiddyStore();
        private List<tblEmployee> dtEmployee;
        private List<tblToy> dtToy;


        public frmMain()
        {
            InitializeComponent();
        }
        public frmMain(string username)
        {
            InitializeComponent();
            dtEmployee = data.tblEmployees.Where(em => em.role.Equals("Employee") && em.isActived == true).ToList();
            dtToy = data.tblToys.Where(toy => toy.isActived == true).ToList();
            emp = data.tblEmployees.Single(em => em.username.Equals(username));
        }
        //Add Employee button
        private void button2_Click(object sender, EventArgs e)
        {
            SetEmployeeTabBlank();
            btnEmployeeSave.Enabled = true;
            gvEmployee.ClearSelection();
        }

        #region Employee_functions
        //<----------------------------Employee function----------------------------------->

        private void SetEmployeeTabBlank()
        {
            txtUsername.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }

        private bool CheckEmployeeTabBlank()
        {
            bool result = false;
            if (txtUsername.Text.Equals(""))
            {
                txtUsername.Focus();
                result = true;
            }
            if (txtFirstName.Text.Equals(""))
            {
                txtUsername.Focus();
                result = true;
            }
            if (txtLastName.Text.Equals(""))
            {
                txtUsername.Focus();
                result = true;
            }
            return result;
        }

        private bool CheckEmployeeExistedUsername(string username)
        {
            bool result = false;
            try
            {
                tblEmployee emp = data.tblEmployees.Single(em => em.username.Equals(username));
                result = true;
            }
            catch (Exception)
            {


            }

            return result;
        }

        private bool CheckValidDate(string date)
        {
            bool result = true;
            try
            {
                DateTime dateTime = DateTime.Parse(date);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;

        }

        private void SetUpEmployeeData()
        {
            LoadEmployeeData(dtEmployee);
            dtDOB.Format = DateTimePickerFormat.Custom;
            dtDOB.CustomFormat = "yyyy-MMM-dd";
            btnEmployeeSave.Enabled = false;
            if (emp.role.Equals("Employee"))
            {
                TabControl.TabPages.Remove(TPEmployee);
            }
        }

        private void LoadEmployeeData(List<tblEmployee> listOfEmp)
        {

            gvEmployee.DataSource = listOfEmp;
            gvEmployee.Columns["username"].HeaderText = "Username";
            gvEmployee.Columns["firstname"].HeaderText = "First name";
            gvEmployee.Columns["lastname"].HeaderText = "Last name";
            gvEmployee.Columns["Role"].Visible = false;
            gvEmployee.Columns["password"].Visible = false;
            gvEmployee.Columns["dob"].Visible = false;
            gvEmployee.Columns["gender"].Visible = false;
            gvEmployee.Columns["isActived"].Visible = false;
            gvEmployee.Columns["image"].Visible = false;
            gvEmployee.Columns["tblFeedbacks"].Visible = false;
            gvEmployee.Columns["tblOrders"].Visible = false;
            gvEmployee.Columns["tblToys"].Visible = false;


        }

        //private void DataBinding()
        //{
        //    txtUsername.DataBindings.Clear();
        //    txtLastName.DataBindings.Clear();
        //    txtFirstName.DataBindings.Clear();
        //    dtDOB.DataBindings.Clear();
        //    rdFemail.DataBindings.Clear();
        //    rdMale.DataBindings.Clear();
        //    txtUsername.DataBindings.Add("Text", dtEmployee, "Username");
        //    txtLastName.DataBindings.Add("Text", dtEmployee, "lastname");
        //    txtFirstName.DataBindings.Add("Text", dtEmployee, "firstname");
            

        //}

        private void SaveEmployee()
        {
            try
            {
                if (!CheckEmployeeTabBlank())
                {
                    if (!CheckEmployeeExistedUsername(txtUsername.Text))
                    {
                        if (CheckValidDate(dtDOB.Text))
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
                            addEmp.password = "1";
                            addEmp.dob = mydob;
                            addEmp.gender = myGender;
                            addEmp.role = "Employee";
                            addEmp.isActived = true;
                            addEmp.firstname = txtFirstName.Text;
                            addEmp.lastname = txtLastName.Text;
                            data.tblEmployees.Add(addEmp);
                            data.SaveChanges();
                            dtEmployee = data.tblEmployees.Where(em => em.role.Equals("Employee") && em.isActived == true).ToList();
                            LoadEmployeeData(dtEmployee);

                            MessageBox.Show("Save employee complete!");
                        }
                        else
                        {
                            MessageBox.Show("Invalid date");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username has already existed!");
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void EditEmployee()
        {
            try
            {
                if (!CheckEmployeeTabBlank())
                {
                    if (CheckEmployeeExistedUsername(txtUsername.Text))
                    {
                        if (CheckValidDate(dtDOB.Text))
                        {
                            DateTime myDate = DateTime.Parse(dtDOB.Text);

                            string mydob = string.Format("{0}-{1}-{2}", myDate.Year, myDate.Month, myDate.Day);
                            string myGender = "female";
                            if (rdMale.Checked == true)
                            {
                                myGender = "male";
                            }
                            tblEmployee editEmp = data.tblEmployees.Single(emp => emp.username.Equals(txtUsername.Text));
                            editEmp.firstname = txtFirstName.Text;
                            editEmp.lastname = txtLastName.Text;
                            editEmp.dob = mydob;
                            editEmp.gender = myGender;
                            data.SaveChanges();
                            dtEmployee = data.tblEmployees.Where(em => em.role.Equals("Employee") && em.isActived == true).ToList();
                            LoadEmployeeData(dtEmployee);
                            MessageBox.Show("Edit employee complete!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username is not existed!");
                    }

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }


        private void DeleteEmployee()
        {
            string username = txtUsername.Text;
            try
            {
                if (CheckEmployeeExistedUsername(txtUsername.Text))
                {
                    tblEmployee delEmp = data.tblEmployees.Single(emp => emp.username.Equals(username));
                    delEmp.isActived = false;
                    data.SaveChanges();
                    dtEmployee = data.tblEmployees.Where(em => em.role.Equals("Employee") && em.isActived == true).ToList();
                    LoadEmployeeData(dtEmployee);
                    MessageBox.Show("Delete employee complete");
                }
                else
                {
                    MessageBox.Show("Username is not existed!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        //<----------------------------end------------------------------------------------>     
        #endregion

        #region Customer_functions
        //<----------------------------Customer function----------------------------------->

        private void LoadCustomerData()
        {
            gvCustomer.DataSource = data.tblCustomers.Where(cus => cus.isActived == true).Select(cus => new
            {
                username = cus.username,
                name = cus.firstname + " " + cus.lastname

            }).ToList();
        }

        private void ViewCustomerOrders(string cusID)
        {
            gvOrders.DataSource = data.tblOrders.Where(ord => ord.cusID.Equals(cusID)).Select(ord => new
            {
                id = ord.id,
                Date = ord.datetime,
                Payment = ord.payment
            }).ToList();
            gvOrders.Columns["id"].Visible = false;
        }
        private void ViewOrderDetail(int orderIDRef)
        {
            gvOrderDetail.ColumnCount = 2;
            gvOrderDetail.Columns[0].Name = "Toy";
            gvOrderDetail.Columns[1].Name = "Quantity";
            List<tblOrderDetail> listOfOrderDetails= data.tblOrderDetails.Where(ord => ord.orderID == orderIDRef).ToList();
            foreach(tblOrderDetail orderDetail in listOfOrderDetails)
            {
                ArrayList row = new ArrayList();
                string toyName = data.tblToys.Single(toy => toy.id == orderDetail.toyID).name;
                row.Add(toyName);
                row.Add(orderDetail.quantity);
                gvOrderDetail.Rows.Add(row.ToArray());
            }
        }


        //</----------------------------end------------------------------------------------> 
        #endregion

        //<----------------------Order&Feedback------------------------------------->
        #region Order&Feedback_functions
        private void SetUpConfirmOrder()
        {
            gvConfirmOrder.Rows.Clear();
            gvConfirmOrder.ColumnCount = 4;
            gvConfirmOrder.Columns[0].Name = "Customer";
            gvConfirmOrder.Columns[1].Name = "Date";
            gvConfirmOrder.Columns[2].Name = "Address";
            gvConfirmOrder.Columns[3].Name = "id";

            List<tblOrder> listOfOrders = data.tblOrders.Where(ord => ord.payment.Equals("confirm")).ToList();
            foreach(tblOrder ord in listOfOrders)
            {
                ArrayList row = new ArrayList();
                string cusName = data.tblCustomers.Single(cus => cus.username.Equals(ord.cusID)).firstname + " " + data.tblCustomers.Single(cus => cus.username.Equals(ord.cusID)).lastname;
                row.Add(cusName);
                row.Add(ord.datetime);
                row.Add(ord.address);
                row.Add(ord.id);
                gvConfirmOrder.Rows.Add(row.ToArray());
            }
            gvConfirmOrder.Columns["id"].Visible = false;
        }
        private void ViewOrderDetail2(int orderIDRef)
        {
            gvOrderDetail2.ColumnCount = 2;
            gvOrderDetail2.Columns[0].Name = "Toy";
            gvOrderDetail2.Columns[1].Name = "Quantity";
            List<tblOrderDetail> listOfOrderDetails = data.tblOrderDetails.Where(ord => ord.orderID == orderIDRef).ToList();
            int i = 0;
            foreach (tblOrderDetail orderDetail in listOfOrderDetails)
            {
                string toyName = data.tblToys.Single(toy => toy.id == orderDetail.toyID).name;
                if (toyName == "") { break; };
                ArrayList row = new ArrayList();      
                row.Add(toyName);
                row.Add(orderDetail.quantity);
                gvOrderDetail2.Rows.Add(row.ToArray());
                i++;
            }
            
        }
        private void ConfirmOrder()
        {
            tblOrder ord = data.tblOrders.Single(order => order.id == ordIDConfirm);
            ord.payment = "success";
            data.SaveChanges();
            SetUpConfirmOrder();
            gvOrderDetail2.Rows.Clear();
        }
        private void RejectOrder()
        {
            tblOrder ord = data.tblOrders.Single(order => order.id == ordIDConfirm);
            ord.payment = "fail";
            data.SaveChanges();
            SetUpConfirmOrder();
            gvOrderDetail2.Rows.Clear();
        }

        #endregion
        //<----------------------end------------------------------------------------->

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            btnEmployeeSave.Enabled = false;
            string gender = gvEmployee.Rows[e.RowIndex].Cells["gender"].Value.ToString();
            txtUsername.Text  = gvEmployee.Rows[e.RowIndex].Cells["username"].Value.ToString();
            txtFirstName.Text  = gvEmployee.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
            txtLastName.Text  = gvEmployee.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
            if (gender.Equals("female"))
            {
                rdFemail.Checked = true;
            }
            else
            {
                rdMale.Checked = true;
            }

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            SetUpEmployeeData();
            LoadCustomerData();
            SetUpConfirmOrder();
        }
       
        
       
        private void btnEmployeeSave_Click(object sender, EventArgs e)
        {
           
            SaveEmployee();
            
        }

        private void btnEmployeeEdit_Click(object sender, EventArgs e)
        {
            EditEmployee();


        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            DeleteEmployee();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            dtEmployee = data.tblEmployees.Where(em => em.role.Equals("Employee") && em.isActived == true && (em.firstname + " " + em.lastname).Contains(txtSearch.Text)).ToList();
            LoadEmployeeData(dtEmployee);
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void gvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string cusID = gvCustomer.CurrentRow.Cells[0].Value.ToString();
            ViewCustomerOrders(cusID);
            gvOrderDetail.DataSource = null;
        }

        private void gvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int orderID = int.Parse(gvOrders.CurrentRow.Cells[0].Value.ToString());
            ViewOrderDetail(orderID);
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm CPasswordform = new ChangePasswordForm(emp.username);
            CPasswordform.Show();
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (txtUsername.Text.Equals(""))
            {
                usernameValidate.SetError(txtUsername, "Username can not be blank!");
            }
            else
            {
                this.usernameValidate.SetError(txtUsername, "");
            }
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (txtFirstName.Text.Equals(""))
            {
                firstnameValidate.SetError(txtFirstName, "Firstname can not be blank!");
            }
            else
            {
                this.firstnameValidate.SetError(txtFirstName, "");
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (txtLastName.Text.Equals(""))
            {
                lastnameValidate.SetError(txtLastName, "Firstname can not be blank!");
            }
            else
            {
                this.lastnameValidate.SetError(txtLastName, "");
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            gvCustomer.DataSource = data.tblCustomers.Where(cus => cus.isActived == true && (cus.firstname + " " + cus.lastname).Contains(txtCustomerSearch.Text)).Select(cus => new
            {
                username = cus.username,
                name = cus.firstname + " " + cus.lastname

            }).ToList();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
            
        }

        private void gvConfirmOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ordIDConfirm = int.Parse(gvConfirmOrder.Rows[e.RowIndex].Cells[3].Value.ToString());
            ViewOrderDetail2(ordIDConfirm);
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ConfirmOrder();
        }

        private void btnRejectOrder_Click(object sender, EventArgs e)
        {
            RejectOrder();
        }
    }
    
    
}

