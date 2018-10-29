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
    public class tblToysController : Controller
    {
        private ToyDBModel db = new ToyDBModel();

        // GET: tblToys
        public ActionResult Index()
        {
            ViewBag.Page = "Index";
            return View(db.tblToys.ToList());
        }

        public ActionResult Login()
        {
            return RedirectToAction("Login", "tblCustomers");
        }

        // GET: tblToys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblToy tblToy = db.tblToys.Find(id);
            if (tblToy == null)
            {
                return HttpNotFound();
            }
            ViewBag.Page = "Details";
            var relatedProduct = db.tblToys.OrderByDescending(toy => toy.id)
                .Where(toy => toy.category == tblToy.category && toy.id != id && toy.isActived == true)
                .Take(4).ToList();
            ViewBag.RelatedProduct = relatedProduct;
            return View(tblToy);
        }

        public ActionResult Category(string category)
        {
            if(category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(!category.Equals("Lego") && !category.Equals("BoardGame") && !category.Equals("Rubik"))
            {
                return HttpNotFound();
            }
            ViewBag.Page = "Category";
            return View(db.tblToys.Where(toy => toy.category == category).ToList());
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
