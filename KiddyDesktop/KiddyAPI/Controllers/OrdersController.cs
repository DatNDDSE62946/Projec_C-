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
    public class OrdersController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Orders
        public IEnumerable<OrderDTO> GettblOrders()
        {
            return db.tblOrders.Select(ord => new OrderDTO
            {
                id= ord.id,
                date = ord.date,
                cusID = ord.cusID,
                address = ord.address,
                payment = ord.payment,
                emlID = ord.emlID,
                status = ord.status
                
            }).ToList();
        }

        // GET: api/Orders/5
        [ResponseType(typeof(tblOrder))]
        public IHttpActionResult GettblOrder(int id)
        {
            tblOrder tblOrder = db.tblOrders.Find(id);
            if (tblOrder == null)
            {
                return NotFound();
            }

            return Ok(tblOrder);
        }
        // GET: api/Orders/OrdersByCusID
        [Route("api/Orders/OrdersByCusID")]
        public IEnumerable<OrderDTO> GetListOrdersByCustomerId(string cusID)
        {
            return db.tblOrders.Where(ord => ord.cusID.Equals(cusID)).Select(ord => new OrderDTO
            {
                id = ord.id,
                date = ord.date,
                status = ord.status,
                payment = ord.payment,
                address = ord.address
            }).ToList();
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttblOrder(int id, OrderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.id)
            {
                return BadRequest();
            }
            tblOrder tblOrder = new tblOrder
            {
                id = dto.id,
                cusID = dto.cusID,
                emlID = dto.emlID,
                payment = dto.payment,
                address = dto.address,
                date = dto.date,
                status = dto.status
            };
            db.Entry(tblOrder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblOrderExists(id))
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

        // POST: api/Orders
        [ResponseType(typeof(tblOrder))]
        public IHttpActionResult PosttblOrder(OrderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            tblOrder order = new tblOrder
            {
                cusID = dto.cusID,
                address = dto.address,
                payment = dto.payment,
                date = DateTime.Now
            };
            db.tblOrders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dto.id }, dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblOrderExists(int id)
        {
            return db.tblOrders.Count(e => e.id == id) > 0;
        }
    }
}