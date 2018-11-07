using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiddyDesktop.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace KiddyDesktop
{
    public partial class frmMain : Form
    {
        private EmployeeDTO emp;
        private KiddyStore data = new KiddyStore();
        private IEnumerable<EmployeeDTO> listEmployees;
        private IEnumerable<ToyDTO> listToys;
        private IEnumerable<CustomerDTO> listCustomer;
        private IEnumerable<ToyDTO> listToyFeedback;
        private IEnumerable<FeedbackDTO> listFeedback;
        private string empImgString;
        private int ordIDConfirm;
        private CustomerDTO currCustomer;
        private static HttpClient client = new HttpClient();
        private static readonly string BASE_URL = "http://localhost:50815/api/";

        public static void initClient()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(EmployeeDTO dto)
        {
            InitializeComponent();
            emp = dto;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
        }


        #region Employee_functions
        //<----------------------------Employee function----------------------------------->
        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (txtUsername.Text.Equals(""))
            {
                usernameValidate.SetError(txtUsername, "Username can not be blank!");
                txtUsername.Focus();
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
                txtFirstName.Focus();

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
                txtLastName.Focus();
            }
            else
            {
                this.lastnameValidate.SetError(txtLastName, "");
            }
        }


        private void SetEmployeeTabBlank()
        {
            txtUsername.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            PBEmployee.Image = null;
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
            if(empImgString == null)
            {
                result = true;
                btnUploadImage.Focus();
            }
            return result;
        }

        private bool CheckEmployeeExistedUsername(string username)
        {
            bool result = false;
            try
            {
                EmployeeDTO emp = listEmployees.Single(em => em.username.Equals(username));
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

        private async void loadEmployees()
        {

            HttpResponseMessage response = await client.GetAsync(BASE_URL + "Employees");
            if (response.IsSuccessStatusCode)
            {

                string strResponse = response.Content.ReadAsStringAsync().Result;
                listEmployees = JsonConvert.DeserializeObject<IEnumerable<EmployeeDTO>>(strResponse);
                LoadEmployeeData(listEmployees);
               
            }
            ClearDataBindingForEmployee();
            bindingSource1.DataSource = listEmployees;
            gvEmployee.DataSource = bindingSource1.DataSource;
            AddDataBindingForEmployee();
        }

        private void LoadEmployeeData(IEnumerable<EmployeeDTO> list)
        {
            gvEmployee.DataSource = list;
            gvEmployee.Columns["image"].Visible = false;
            gvEmployee.Columns["dob"].Visible = false;
            gvEmployee.Columns["isActived"].Visible = false;
            gvEmployee.Columns["gender"].Visible = false;
            gvEmployee.Columns["role"].Visible = false;
            gvEmployee.Columns["password"].Visible = false;
            gvEmployee.Columns["firstname"].Visible = false;
            gvEmployee.Columns[0].HeaderText = "Username";
            gvEmployee.Columns[1].HeaderText = "Name";
        }


        private async void AddEmployeeToDB(EmployeeDTO dto)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(BASE_URL + "Employees", dto);
            try
            {
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Add Employee Success!");
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private async void EditEmployeeToDB(EmployeeDTO dto)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(BASE_URL + "Employees/" + dto.username, dto);
            try
            {
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Edit employee success");
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }


        private async void DeleteEmployee(string username)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(BASE_URL + "Employees/Delete?id=" + username, "");
            try
            {
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Delete employee success");
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }


        private void SetUpEmployeeData()
        {
           
            dtDOB.Format = DateTimePickerFormat.Custom;
            dtDOB.CustomFormat = "yyyy-MMM-dd";
            btnEmployeeSave.Enabled = false;
            if (emp.role.Equals("Employee"))
            {
                TabControl.TabPages.Remove(tabEmployee);
            }
            
           
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private void SaveEmployee()
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
                            Image img = Image.FromFile(empImgString);
                            byte[] imgByte = imageToByteArray(img);
                            EmployeeDTO addEmp = new EmployeeDTO();
                            addEmp.username = txtUsername.Text;
                            addEmp.password = "1";
                            addEmp.dob = mydob;
                            addEmp.gender = myGender;
                            addEmp.role = "Employee";
                            addEmp.isActived = true;
                            addEmp.firstname = txtFirstName.Text;
                            addEmp.lastname = txtLastName.Text;
                            addEmp.image = imgByte;
                            AddEmployeeToDB(addEmp);
                         
                            
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

        private void EditEmployee()
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
                            Image image = Image.FromFile(empImgString);
                            byte[] imagebyte = imageToByteArray(image);
                            EmployeeDTO editEmp = new EmployeeDTO();
                            editEmp.username = txtUsername.Text;
                            editEmp.firstname = txtFirstName.Text;
                            editEmp.lastname = txtLastName.Text;
                            editEmp.dob = mydob;
                            editEmp.gender = myGender;
                            editEmp.image = imagebyte;
                            EditEmployeeToDB(editEmp);
                          
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username is not existed!");
                    }

                }
            
                   }

        private void DeleteEmployee()
        {
            string username = txtUsername.Text;
          
                if (CheckEmployeeExistedUsername(txtUsername.Text))
                {
                DeleteEmployee(txtUsername.Text);
                    SetEmployeeTabBlank();
                }
                else
                {
                    MessageBox.Show("Username is not existed!");
                }
            
        }

        private void ClearDataBindingForEmployee()
        {
            txtUsername.DataBindings.Clear();
            txtFirstName.DataBindings.Clear();
            txtLastName.DataBindings.Clear();
            dtDOB.DataBindings.Clear();
            PBEmployee.DataBindings.Clear();
            rdMale.DataBindings.Clear();
            rdFemail.DataBindings.Clear();
        }

        private void AddDataBindingForEmployee()
        {
            txtUsername.DataBindings.Add("Text", listEmployees, "username");
            txtFirstName.DataBindings.Add("Text", listEmployees, "firstname");
            txtLastName.DataBindings.Add("Text", listEmployees, "lastname");
            dtDOB.DataBindings.Add("Text", listEmployees, "dob");
            PBEmployee.DataBindings.Add("Image", listEmployees, "image", true);
            //Binding maleBinding = new Binding("Checked", listEmployees, "gender");
            //maleBinding.Format += (s, args) => args.Value = ((string)args.Value) == "male";
            //maleBinding.Parse += (s, args) => args.Value = (bool)args.Value ? "male" : "female";
            //rdMale.DataBindings.Add(maleBinding);
            //Binding femaleBinding = new Binding("Checked", listEmployees, "gender");
            //femaleBinding.Format += (s, args) => args.Value = ((string)args.Value) == "female";
            //femaleBinding.Parse += (s, args) => args.Value = (bool)args.Value ? "male" : "female";
            //rdFemail.DataBindings.Add(femaleBinding);

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            loadToys();
            loadEmployees();
            SetUpEmployeeData();
            LoadCustomerData();
            SetUpConfirmOrder();

        }


        //Add Employee button
        private void button2_Click(object sender, EventArgs e)
        {
            SetEmployeeTabBlank();
            btnEmployeeSave.Enabled = true;
            gvEmployee.ClearSelection();
            empImgString = null;
            ordIDConfirm = -1;
            txtUsername.Enabled = true;

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            txtUsername.Enabled = false;
        }


        private void btnEmployeeSave_Click(object sender, EventArgs e)
        {
            SaveEmployee();
            loadEmployees();
        }

        private void btnEmployeeEdit_Click(object sender, EventArgs e)
        {
            EditEmployee();
            loadEmployees();
        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            DeleteEmployee(txtUsername.Text);
            loadEmployees();
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<EmployeeDTO> searchListEmployees = listEmployees.Where(em => (em.firstname + " " + em.lastname).Contains(txtSearch.Text)).ToList();
            LoadEmployeeData(searchListEmployees);
        }


        //<----------------------------end------------------------------------------------>     
        #endregion

        #region Customer_functions
        //<----------------------------Customer function----------------------------------->
        private async void LoadCustomerData()
        {
            HttpResponseMessage response = await client.GetAsync(BASE_URL + "Customers");
            if (response.IsSuccessStatusCode)
            {
                string strResponse = response.Content.ReadAsStringAsync().Result;
                listCustomer = JsonConvert.DeserializeObject<IEnumerable<CustomerDTO>>(strResponse);
                
            }
            gvCustomer.DataSource = listCustomer.Select(cus => new
            {
                username = cus.username,
                name = cus.firstname + " " + cus.lastname

            }).ToList();
            
        }

        private async void LoadCustomerOrders(string cusID)
        {
            HttpResponseMessage response = await client.GetAsync(BASE_URL + "Orders/OrdersByCusID?cusID=" + cusID);
            IEnumerable<OrderDTO> listOrders = null;
            if (response.IsSuccessStatusCode)
            {
                string strResponse = response.Content.ReadAsStringAsync().Result;
                listOrders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(strResponse);
            }
            gvOrders.DataSource = listOrders;
            gvOrders.Columns["cusID"].Visible = false;
            gvOrders.Columns["emlID"].Visible = false;
            gvOrders.Columns["address"].Visible = false;
            gvOrders.Columns["payment"].Visible = false;
            gvOrders.Columns["ID"].Visible = false;
        }

        private async void ViewOrderDetail(int orderIDRef)
        {
            IEnumerable<OrderDetailDTO> listOfOrderDetails = null;
            HttpResponseMessage response= await client.GetAsync(BASE_URL + "OrderDetails/OrderDetailsByOrderID?orderID=" + orderIDRef);
            if (response.IsSuccessStatusCode)
            {
                string strResponse = response.Content.ReadAsStringAsync().Result;
                listOfOrderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetailDTO>>(strResponse);
            }
                
            gvOrderDetail.ColumnCount = 2;
            gvOrderDetail.Columns[0].Name = "Toy";
            gvOrderDetail.Columns[1].Name = "Quantity";
            gvOrderDetail.Rows.Clear();
               
            foreach (OrderDetailDTO orderDetail in listOfOrderDetails)
            {
                ArrayList row = new ArrayList();
                string toyName = listToys.Single(toy => toy.id == orderDetail.toyID).name;
                row.Add(toyName);
                row.Add(orderDetail.quantity);
                gvOrderDetail.Rows.Add(row.ToArray());
            }
            
            
        }

        private async void BlockCustomer(string id)
        {
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(BASE_URL + "Customers?id=" + id, "");
            string strResponse = responseMessage.Content.ReadAsStringAsync().Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                currCustomer = JsonConvert.DeserializeObject<CustomerDTO>(strResponse);
            }
            currCustomer.isActive = false;
            HttpResponseMessage responseMessage2 = await client.PutAsJsonAsync(BASE_URL + "Customers?id=" + currCustomer.username, currCustomer);
            try
            {
                responseMessage2.EnsureSuccessStatusCode();
                MessageBox.Show("Block Customer success!");
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }
        

        private void btnBlock_Click(object sender, EventArgs e)
        {
            string cusID = gvCustomer.CurrentRow.Cells[0].Value.ToString();
            BlockCustomer(cusID);
            LoadCustomerData();
           

        }

        private void gvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gvOrderDetail.DataSource = null;
            string cusID = gvCustomer.CurrentRow.Cells[0].Value.ToString();
            LoadCustomerOrders(cusID);
            

        }

        private void gvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int orderID = int.Parse(gvOrders.CurrentRow.Cells[0].Value.ToString());
            ViewOrderDetail(orderID);
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            gvCustomer.DataSource = listCustomer.Where(cus =>  (cus.firstname + " " + cus.lastname).Contains(txtCustomerSearch.Text)).Select(cus => new
            {
                username = cus.username,
                name = cus.firstname + " " + cus.lastname

            }).ToList();
        }


        //</----------------------------end------------------------------------------------> 
        #endregion

        #region Order&Feedback_functions
        private async void SetUpConfirmOrder()
        {
            gvConfirmOrder.Rows.Clear();
            gvConfirmOrder.ColumnCount = 4;
            gvConfirmOrder.Columns[0].Name = "Customer";
            gvConfirmOrder.Columns[1].Name = "Date";
            gvConfirmOrder.Columns[2].Name = "Address";
            gvConfirmOrder.Columns[3].Name = "id";
            HttpResponseMessage responseMessage = await client.GetAsync(BASE_URL + "Orders");
            string strResponse = responseMessage.Content.ReadAsStringAsync().Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                IEnumerable<OrderDTO> listOfOrders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(strResponse);
                foreach (OrderDTO ord in listOfOrders)
                {
                    ArrayList row = new ArrayList();
                    //string cusName = data.tblCustomers.Single(cus => cus.username.Equals(ord.cusID)).firstname + " " + data.tblCustomers.Single(cus => cus.username.Equals(ord.cusID)).lastname;
                    row.Add(ord.cusID);
                    row.Add(ord.datetime);
                    row.Add(ord.address);
                    row.Add(ord.id);
                    gvConfirmOrder.Rows.Add(row.ToArray());
                }
                gvConfirmOrder.Columns["id"].Visible = false;
            }
            
        }
        private void ViewOrderDetail2(int orderIDRef)
        {
            gvOrderDetail2.ColumnCount = 2;
            gvOrderDetail2.Columns[0].Name = "Toy";
            gvOrderDetail2.Columns[1].Name = "Quantity";
            gvOrderDetail2.Rows.Clear();
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


        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm CPasswordform = new ChangePasswordForm(emp.username);
            CPasswordform.Show();
        }

     
      

        private void gvConfirmOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ordIDConfirm = int.Parse(gvConfirmOrder.Rows[e.RowIndex].Cells[3].Value.ToString());
            ViewOrderDetail2(ordIDConfirm);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (ordIDConfirm != -1)
            {
                ConfirmOrder();
            }
        }

        private void btnRejectOrder_Click(object sender, EventArgs e)
        {
            RejectOrder();
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Title = "Please select a photo";
            fileChooser.Filter = "JPG|*.jpg|PNG|*.png|GIF|*gif";
            fileChooser.Multiselect = false;
            if(fileChooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.PBEmployee.ImageLocation = fileChooser.FileName;
                empImgString = fileChooser.FileName;
                PBEmployee.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btnUploadImage_Validating(object sender, CancelEventArgs e)
        {
            if(empImgString == null && PBEmployee.Image == null)
            {
                imageValidate.SetError(btnUploadImage, "Please choose employee image");
                btnUploadImage.Focus();
            }
            else
            {
                imageValidate.SetError(btnUploadImage, "");
            }
        }
       
        #region Toy_Functions
        private async void loadToys()
        {
            HttpResponseMessage response = await client.GetAsync(BASE_URL + "Toys");
            if(response.IsSuccessStatusCode)
            {
                string strResponse = response.Content.ReadAsStringAsync().Result;
                listToys = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(strResponse);
                dgvProducts.DataSource = null;
                dgvProducts.DataSource = listToys;
                dgvProducts.Columns["image"].Visible = false;
                dgvProducts.Columns["createdBy"].Visible = false;
                dgvProducts.Columns["isActived"].Visible = false;
                dgvProducts.Columns["description"].Visible = false;
                dgvProducts.Columns["category"].Visible = false;
                clearDataBindingForProduct();
                addDataBindingForProduct();
            } else
            {
                MessageBox.Show("Load product failed! Please check your connection!");
            }
        }

        private async void addToy(ToyDTO dto)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(BASE_URL + "Toys", dto);
            try
            {
                response.EnsureSuccessStatusCode();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private async void updateToy(ToyDTO dto)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(BASE_URL + "Toys/" + dto.id, dto);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void deleteToy(int id)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(BASE_URL + "Toys/Delete?id=" + id, "");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearPro_Click(object sender, EventArgs e)
        {
            clearDataBindingForProduct();

            txtProID.Text = "";
            txtProName.Text = "";
            txtProPrice.Text = "";
            txtProQuantity.Text = "";
            txtProDescription.Text = "";
            cbProCategory.SelectedIndex = 0;
            pbProImage.Image = null;
        }
        

        private void addDataBindingForProduct()
        {
            txtProID.DataBindings.Add("Text", listToys, "id");
            txtProName.DataBindings.Add("Text", listToys, "name");
            txtProPrice.DataBindings.Add("Text", listToys, "price");
            txtProQuantity.DataBindings.Add("Text", listToys, "quantity");
            txtProDescription.DataBindings.Add("Text", listToys, "description");
            cbProCategory.DataBindings.Add("Text", listToys, "category");
            pbProImage.DataBindings.Add("Image", listToys, "image", true);

        }

        private void clearDataBindingForProduct()
        {
            txtProID.DataBindings.Clear();
            txtProName.DataBindings.Clear();
            txtProPrice.DataBindings.Clear();
            txtProQuantity.DataBindings.Clear();
            txtProDescription.DataBindings.Clear();
            cbProCategory.DataBindings.Clear();
            pbProImage.DataBindings.Clear();
        }

        private void btnAddPro_Click(object sender, EventArgs e)
        {
            string proName, proCategory, proDescription;
            float proPrice = -1;
            int proQuantity = -1;
            byte[] image = null;
            bool check = true;
            proName = txtProName.Text.Trim();
            proCategory = (string)cbProCategory.SelectedItem;
            proDescription = txtProDescription.Text.Trim();
            if(proName.Length == 0 || proCategory.Length == 0 || proDescription.Length == 0)
            {
                check = false;
            }
            try
            {
                proPrice = float.Parse(txtProPrice.Text.Trim());
                if(proPrice <= 0)
                {
                    check = false;
                }
            } catch (Exception ex)
            {
                check = false;
            }
            try
            {
                proQuantity = int.Parse(txtProQuantity.Text.Trim());
                if(proQuantity <= 0)
                {
                    check = false;
                }
            } catch (Exception ex)
            {
                check = false;
            }
            if (pbProImage.Image == null)
            {
                check = false;
            } else
            {
                image = imageToByteArray(pbProImage.Image);
            }
            
            if(check)
            {
                ToyDTO dto = new ToyDTO
                {
                    name = proName,
                    price = proPrice,
                    quantity = proQuantity,
                    category = proCategory,
                    description = proDescription,
                    image = image
                };

                addToy(dto);
                MessageBox.Show("Add a toy success!");
                btnClearPro_Click(sender, e);
                loadToys();
            } else
            {
                MessageBox.Show("Your input is invalid! Please try again!");
            }
            
        }

        private void dgvProducts_MouseClick(object sender, MouseEventArgs e)
        {
            clearDataBindingForProduct();
            addDataBindingForProduct();
        }

        private void btnSavePro_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProID.Text);
            string proName = txtProName.Text.Trim();
            float proPrice = float.Parse(txtProPrice.Text.Trim());
            int proQuantity = int.Parse(txtProQuantity.Text.Trim());
            string proCategory = (string)cbProCategory.SelectedItem;
            string proDescription = txtProDescription.Text.Trim();
            byte[] image = imageToByteArray(pbProImage.Image);

            ToyDTO dto = new ToyDTO
            {
                id = id,
                name = proName,
                price = proPrice,
                quantity = proQuantity,
                category = proCategory,
                description = proDescription,
                image = image
            };

            updateToy(dto);
            MessageBox.Show("Update toy id" + id + " success!");
            btnClearPro_Click(sender, e);
            loadToys();
        }

        private void btnDeletePro_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProID.Text);
            DialogResult confirm = MessageBox.Show("Are you sure to delete toy id " + id + " ?",
                                                    "Confirmation", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                deleteToy(id);
                MessageBox.Show("Delete toy id " + id + " success!");
                loadToys();
            }
        }

        private void txtProName_Enter(object sender, EventArgs e)
        {
            txtProName.DataBindings.Clear();
        }

        private void txtProPrice_Enter(object sender, EventArgs e)
        {
            txtProPrice.DataBindings.Clear();
        }

        private void txtProQuantity_Enter(object sender, EventArgs e)
        {
            txtProQuantity.DataBindings.Clear();
        }

        private void txtProDescription_Enter(object sender, EventArgs e)
        {
            txtProDescription.DataBindings.Clear();
        }

        private void txtSearchPro_TextChanged(object sender, EventArgs e)
        {
            //clearDataBindingForProduct();
            //string search = txtSearchPro.Text;
            //BindingSource bs = new BindingSource();
            //bs.DataSource = dgvProducts.DataSource;
            //bs.Filter = "[name] = '" + search + "'";
            //dgvProducts.DataSource = bs;
        }

        private void btnUploadProImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Title = "Please select a photo";
            fileChooser.Filter = "JPG|*.jpg|PNG|*.png|GIF|*gif";
            fileChooser.Multiselect = false;
            if (fileChooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pbProImage.ImageLocation = fileChooser.FileName;
                pbProImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void txtProName_Validating(object sender, CancelEventArgs e)
        {
            if (txtProName.Text == "")
            {
                errProduct.SetError(txtProName, "Name is required!");
            }
            else
            {
                errProduct.SetError(txtProName, "");
            }
        }

        private void txtProPrice_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                float price = float.Parse(txtProPrice.Text);
                errProduct.SetError(txtProPrice, "");
            }
            catch (Exception ex)
            {
                errProduct.SetError(txtProPrice, "Price must be a number!");
            }
        }

        private void txtProQuantity_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int price = int.Parse(txtProQuantity.Text);
                errProduct.SetError(txtProQuantity, "");
            }
            catch (Exception ex)
            {
                errProduct.SetError(txtProQuantity, "Quantity must be a number!");
            }
        }

        private void txtProDescription_Validating(object sender, CancelEventArgs e)
        {
            if (txtProDescription.Text == "")
            {
                errProduct.SetError(txtProDescription, "Description is required!");
            }
            else
            {
                errProduct.SetError(txtProDescription, "");
            }
        }

        private void btnUploadProImage_Validating(object sender, CancelEventArgs e)
        {
            if (pbProImage.Image == null)
            {
                errProduct.SetError(btnUploadProImage, "Image is required!");
            }
            else
            {
                errProduct.SetError(btnUploadProImage, "");
            }
        }

        #endregion

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            EmployeeDTO currEmp = (EmployeeDTO) bindingSource1.Current ;

            if (currEmp.gender.Equals("male"))
            {
                rdMale.Checked = true;
            }
            else
            {
                rdFemail.Checked = true;
            }
        }

        #region Feedback_Functions
        private void tabOrderFeedback_Enter(object sender, EventArgs e)
        {
            loadToyFeedback();
            loadFeedback();
        }

        private async void loadToyFeedback()
        {
            HttpResponseMessage response = await client.GetAsync(BASE_URL + "Toys\\Feedbacks");
            string stringResponse = response.Content.ReadAsStringAsync().Result;
            listToyFeedback = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(stringResponse);
            dgvProFeedback.DataSource = null;
            dgvProFeedback.DataSource = listToyFeedback;

            if (listToyFeedback != null)
            {
                dgvProFeedback.Columns["image"].Visible = false;
                dgvProFeedback.Columns["createdBy"].Visible = false;
                dgvProFeedback.Columns["isActived"].Visible = false;
                dgvProFeedback.Columns["description"].Visible = false;
                dgvProFeedback.Columns["category"].Visible = false;
                dgvProFeedback.Columns["price"].Visible = false;
                dgvProFeedback.Columns["quantity"].Visible = false;
            }
        }

        private async void loadFeedback()
        {
            HttpResponseMessage response = await client.GetAsync(BASE_URL + "Feedbacks");
            string stringResponse = response.Content.ReadAsStringAsync().Result;
            listFeedback = JsonConvert.DeserializeObject<IEnumerable<FeedbackDTO>>(stringResponse);
            dgvFeedback.DataSource = null;
        }

        private async void updateFeedback(FeedbackDTO dto)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(BASE_URL + "Feedbacks/" + dto.id, dto);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvProFeedback_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = e.RowIndex;
            int toyID = (int)dgvProFeedback.Rows[currentRow].Cells["id"].Value;
            List<FeedbackDTO> feedbacks = new List<FeedbackDTO>();
            foreach (FeedbackDTO feedback in listFeedback)
            {
                if (feedback.toyID == toyID)
                {
                    feedbacks.Add(feedback);
                }
            }
            dgvFeedback.DataSource = null;
            dgvFeedback.DataSource = feedbacks;

            dgvFeedback.Columns["toyID"].Visible = false;
            dgvFeedback.Columns["content"].Visible = false;
            dgvFeedback.Columns["status"].Visible = false;
            dgvFeedback.Columns["id"].Width = 20;

            clearDatabindingForFeedback();
            addDataBindingForFeedback(feedbacks);
        }

        private void clearDatabindingForFeedback()
        {
            txtFeedback.DataBindings.Clear();
        }

        private void addDataBindingForFeedback(List<FeedbackDTO> feedbacks)
        {
            txtFeedback.DataBindings.Add("Text", feedbacks, "content");
        }

        private void btnConfirmFeedback_Click(object sender, EventArgs e)
        {
            if (dgvFeedback.CurrentRow == null)
            {
                if (listToyFeedback.Count() == 0)
                {
                    MessageBox.Show("There are no feedback to confirm!");
                } else
                {
                    MessageBox.Show("Please choose a feedback to confirm!");
                }
            } else {
                int currentRowIndex = dgvFeedback.CurrentRow.Index;
                int feedbackID = (int)dgvFeedback.Rows[currentRowIndex].Cells["id"].Value;
                FeedbackDTO dto = new FeedbackDTO { id = feedbackID, status = 1 };
                updateFeedback(dto);
                updateFeedbackGridview(feedbackID, sender);
            }
        }

        private void btnDeleteFeedback_Click(object sender, EventArgs e)
        {
            if (dgvFeedback.CurrentRow == null)
            {
                if (listToyFeedback.Count() == 0)
                {
                    MessageBox.Show("There are no feedback to confirm!");
                }
                else
                {
                    MessageBox.Show("Please choose a feedback to confirm!");
                }
            }
            else
            {
                int currentRowIndex = dgvFeedback.CurrentRow.Index;
                int feedbackID = (int)dgvFeedback.Rows[currentRowIndex].Cells["id"].Value;
                FeedbackDTO dto = new FeedbackDTO { id = feedbackID, status = -1 };
                updateFeedback(dto);
                updateFeedbackGridview(feedbackID, sender);
            }
        }

        private void updateFeedbackGridview(int feedbackID, Object sender)
        {
            listFeedback = listFeedback.Where(feedback => feedback.id != feedbackID).ToList();
            dgvProFeedback_CellClick(sender, new DataGridViewCellEventArgs(0, dgvProFeedback.CurrentRow.Index));
            if (dgvFeedback.Rows.Count == 0)
            {
                int toyID = (int)dgvProFeedback.Rows[dgvProFeedback.CurrentRow.Index].Cells["id"].Value;
                listToyFeedback = listToyFeedback.Where(toy => toy.id != toyID).ToList();
                dgvProFeedback.DataSource = null;
                dgvProFeedback.DataSource = listToyFeedback;

                dgvProFeedback.Columns["image"].Visible = false;
                dgvProFeedback.Columns["createdBy"].Visible = false;
                dgvProFeedback.Columns["isActived"].Visible = false;
                dgvProFeedback.Columns["description"].Visible = false;
                dgvProFeedback.Columns["category"].Visible = false;
                dgvProFeedback.Columns["price"].Visible = false;
                dgvProFeedback.Columns["quantity"].Visible = false;
            }
        }


        #endregion
    }
}

