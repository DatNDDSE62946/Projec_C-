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
    public class tblToysController : Controller
    {
        private static HttpClient client = new HttpClient();
        private static string baseURL = "http://localhost:50815/api/";

        public async Task<ActionResult> LoadImage()
        {
            IEnumerable<ToyDTO> list = null;
            HttpResponseMessage response = await client.GetAsync(baseURL + "Toys/Newest");
            if (response.IsSuccessStatusCode)
            {
                string listToy = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(listToy);
                foreach (var toy in list)
                {
                    string imageName = "toy_" + toy.id + ".jpg";
                    System.IO.File.WriteAllBytes(Server.MapPath(@"~/Content/images/") + imageName, toy.image);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: tblToys
        public async Task<ActionResult> Index()
        {
            IEnumerable<ToyDTO> list = null;
            HttpResponseMessage response = await client.GetAsync(baseURL + "Toys/Newest");
            if(response.IsSuccessStatusCode)
            {
                string listToy = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(listToy);
            }
            return View(list);
        }

        public ActionResult Login()
        {
            return RedirectToAction("Login", "tblCustomers");
        }

        public ActionResult ChangePassword()
        {
            return RedirectToAction("ChangePassword", "tblCustomers");
        }

        // GET: tblToys/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = await client.GetAsync(baseURL + "Toys" + "\\" + id);
            string strDto = response.Content.ReadAsStringAsync().Result;
            ToyDTO dto = JsonConvert.DeserializeObject<ToyDTO>(strDto);
            if (dto == null)
            {
                return HttpNotFound();
            }
            
            response = await client.GetAsync(baseURL + "Toys" + "?id=" + id + "&related=" + dto.category);
            string strRelated = response.Content.ReadAsStringAsync().Result;
            IEnumerable<ToyDTO> relatedProduct = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(strRelated);
            ViewBag.RelatedProduct = relatedProduct;

            response = await client.GetAsync(baseURL + "Feedbacks\\ToyId?toyId=" + id);
            string strFeedback = response.Content.ReadAsStringAsync().Result;
            IEnumerable<FeedbackDTO> feedbacks = JsonConvert.DeserializeObject<IEnumerable<FeedbackDTO>>(strFeedback);
            ViewBag.Feedbacks = feedbacks;

            return View(dto);
        }

        public async Task<ActionResult> Category(string category)
        {
            IEnumerable<ToyDTO> list = null;
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!category.Equals("Lego") && !category.Equals("BoardGame") && !category.Equals("Rubik"))
            {
                return HttpNotFound();
            }
            HttpResponseMessage response = await client.GetAsync(baseURL + "Toys" + "?category=" + category);
            if(response.IsSuccessStatusCode)
            {
                string strCategory = response.Content.ReadAsStringAsync().Result;
                 list = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(strCategory);
            }
            
            return View(list);
        }

        public ActionResult Logout()
        {
            Session["USER"] = null;
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Search(string value)
        {
            HttpResponseMessage response = await client.GetAsync(baseURL + "Toys/Search?value=" + value);
            string strResponse = response.Content.ReadAsStringAsync().Result;
            IEnumerable<ToyDTO> list = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(strResponse);
            return View(list);
        }
    }
}
