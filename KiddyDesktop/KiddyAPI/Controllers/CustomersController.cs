using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KiddyAPI.Models;

namespace KiddyAPI.Controllers
{
    public class CustomersController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Customers
        public IQueryable<CustomerDTO> GettblCustomers()
        {
            var cusList = db.tblCustomers.Where(cus => cus.isActived == true)
                .Select(cus => new CustomerDTO { username = cus.username, firstname = cus.firstname, lastname = cus.lastname });
            return cusList;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(tblCustomer))]
        public IHttpActionResult GettblCustomer(string id)
        {
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return NotFound();
            }

            return Ok(tblCustomer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttblCustomer(string id, tblCustomer tblCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblCustomer.username)
            {
                return BadRequest();
            }

            db.Entry(tblCustomer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblCustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/Customers/ChangePassword")]
        public IHttpActionResult ChangePassword(CustomerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tblCustomer cus = db.tblCustomers.Find(dto.username);

            if(cus == null)
            {
                return BadRequest();
            }
            cus.password = dto.password;

            db.Entry(cus).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblCustomerExists(dto.username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //// POST: api/Customers
        //[ResponseType(typeof(tblCustomer))]
        //public IHttpActionResult PosttblCustomer(tblCustomer tblCustomer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tblCustomers.Add(tblCustomer);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (tblCustomerExists(tblCustomer.username))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = tblCustomer.username }, tblCustomer);
        //}

        //POST: api/Customers/CheckLogin
        [HttpPost]
        [Route("api/Customers/CheckLogin")]
        [ResponseType(typeof(CustomerDTO))]
        public CustomerDTO checkLogin(CustomerDTO customer)
        {
            CustomerDTO dto = null;
            var cus = db.tblCustomers.SingleOrDefault(c => c.username == customer.username 
                                                       && c.password == customer.password
                                                       && c.isActived == true);
            if(cus != null)
            {
                dto = new CustomerDTO { username = cus.username, lastname = cus.lastname };
            }
            return dto;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblCustomerExists(string id)
        {
            return db.tblCustomers.Count(e => e.username == id) > 0;
        }
    }
}