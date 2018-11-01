using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KiddyWeb.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KiddyWeb.Controllers
{
    public class tblCustomersController : Controller
    {
        static HttpClient client = new HttpClient();
        private string baseURL = "http://localhost:50815/api/Customers/";

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "tblToys");
        }

        [HttpPost]
        public async Task<ActionResult> Login([Bind(Include = "username, password")] CustomerDTO customer)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(baseURL + "CheckLogin", customer);
            string resString = response.Content.ReadAsStringAsync().Result;
            CustomerDTO dto = JsonConvert.DeserializeObject<CustomerDTO>(resString);
            if(dto != null) {
                Session["USER"] = dto.username;
                Session["LASTNAME"] = dto.lastname;
                return RedirectToAction("Index", "tblToys");
            } else
            {
                ViewBag.Invalid = "Invalid email or password!";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string oldpassword, string newpassword)
        {
            string username = Session["USER"].ToString();
            CustomerDTO customer = new CustomerDTO { username = username, password = oldpassword };
            HttpResponseMessage response = await client.PostAsJsonAsync(baseURL + "CheckLogin", customer);
            string resString = response.Content.ReadAsStringAsync().Result;
            CustomerDTO dto = JsonConvert.DeserializeObject<CustomerDTO>(resString);
            if(dto != null)
            {
                dto.password = newpassword;
                response = await client.PutAsJsonAsync(baseURL + "ChangePassword", dto);
                response.EnsureSuccessStatusCode();
                ViewBag.Success = "Change Password Success!";
            } else
            {
                ViewBag.Invalid = "Old password is wrong! Please try again!";
            }
            return View();
        }

        // POST: tblCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "username,password,isActived,firstname,lastname")] tblCustomer tblCustomer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblCustomers.Add(tblCustomer);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tblCustomer);
        //}

        // GET: tblCustomers/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblCustomer tblCustomer = db.tblCustomers.Find(id);
        //    if (tblCustomer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblCustomer);
        //}

        // POST: tblCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "username,password,isActived,firstname,lastname")] tblCustomer tblCustomer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tblCustomer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tblCustomer);
        //}

        // GET: tblCustomers/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblCustomer tblCustomer = db.tblCustomers.Find(id);
        //    if (tblCustomer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblCustomer);
        //}
    }
}
