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
    public class EmployeesController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Employees
        public IEnumerable<EmployeeDTO> GettblEmployees()
        {
            var emList = db.tblEmployees.Where(em => em.isActived == true).Select(em => new EmployeeDTO
            {
                username = em.username,
                dob = em.dob,
                firstname = em.firstname,
                lastname = em.lastname,
                gender = em.gender,
                image = em.image
            }).ToList();
            return emList;
        }

        // GET: api/Employees/5
        [ResponseType(typeof(tblEmployee))]
        public IHttpActionResult GettblEmployee(string id)
        {
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return NotFound();
            }

            return Ok(tblEmployee);
        }
        // GET: api/Employee/

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttblEmployee(string id, tblEmployee tblEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblEmployee.username)
            {
                return BadRequest();
            }

            db.Entry(tblEmployee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblEmployeeExists(id))
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

        // POST: api/Employees
        [ResponseType(typeof(tblEmployee))]
        public IHttpActionResult PosttblEmployee(tblEmployee tblEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tblEmployees.Add(tblEmployee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tblEmployeeExists(tblEmployee.username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tblEmployee.username }, tblEmployee);
        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(tblEmployee))]
        public IHttpActionResult DeletetblEmployee(string id)
        {
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return NotFound();
            }

            db.tblEmployees.Remove(tblEmployee);
            db.SaveChanges();

            return Ok(tblEmployee);
        }

        //POST: api/Employees/CheckLogin
        [HttpPost]
        [Route("api/Employees/CheckLogin")]
        public EmployeeDTO CheckLogin(string username, string password)
        {

            return db.tblEmployees.Select(em => new EmployeeDTO
            {
                username = em.username,
                lastname = em.lastname
            }).Single(em => em.username.Equals(username) && em.password.Equals(password)) ;
        }


        //Code tu sinh------------------------------------
        #region
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblEmployeeExists(string id)
        {
            return db.tblEmployees.Count(e => e.username == id) > 0;
        }
        #endregion
    }
}