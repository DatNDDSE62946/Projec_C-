using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KiddyWeb.Models;

namespace KiddyWeb.Controllers
{
    public class tblCustomersController : Controller
    {
        private ToyDBModel db = new ToyDBModel();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "username, password")] tblCustomer customer)
        {
            tblCustomer cus = db.tblCustomers.Find(customer.username);
            if(cus != null && cus.password.Equals(customer.password)) {
                tblCustomer user = new tblCustomer { username = cus.username, lastname = cus.lastname };
                Session["USER"] = user;
                Session["LASTNAME"] = user.lastname;
                return RedirectToAction("Index", "tblToys");
            } else
            {
                ViewBag.Invalid = "Invalid email or password!";
                return View();
            }
        }

        // POST: tblCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,password,isActived,firstname,lastname")] tblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                db.tblCustomers.Add(tblCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCustomer);
        }

        // GET: tblCustomers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // POST: tblCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,password,isActived,firstname,lastname")] tblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCustomer);
        }

        // GET: tblCustomers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // POST: tblCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            db.tblCustomers.Remove(tblCustomer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
