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
        static HttpClient client = new HttpClient();
        private string baseURL = "http://localhost:50815/api/Toys";

        // GET: tblToys
        public async Task<ActionResult> Index()
        {
            IEnumerable<ToyDTO> list = null;
            HttpResponseMessage response = await client.GetAsync(baseURL);
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

        // GET: tblToys/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = await client.GetAsync(baseURL + "\\" + id);
            string strDto = response.Content.ReadAsStringAsync().Result;
            ToyDTO dto = JsonConvert.DeserializeObject<ToyDTO>(strDto);
            if (dto == null)
            {
                return HttpNotFound();
            }
            
            response = await client.GetAsync(baseURL + "?id=" + id + "&related=" + dto.category);
            string strRelated = response.Content.ReadAsStringAsync().Result;
            IEnumerable<ToyDTO> relatedProduct = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(strRelated);
            ViewBag.RelatedProduct = relatedProduct;
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
            HttpResponseMessage response = await client.GetAsync(baseURL + "?category=" + category);
            if(response.IsSuccessStatusCode)
            {
                string strCategory = response.Content.ReadAsStringAsync().Result;
                 list = JsonConvert.DeserializeObject<IEnumerable<ToyDTO>>(strCategory);
            }
            
            return View(list);
        }


    }
}
