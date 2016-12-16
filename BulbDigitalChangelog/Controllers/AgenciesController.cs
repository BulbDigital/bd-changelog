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
using BulbDigitalChangelog.Models;

namespace BulbDigitalChangelog.Controllers
{
    public class AgenciesController : ApiController
    {
        private BulbDigitalChangelogContext db = new BulbDigitalChangelogContext();

        // GET: api/Agencies
        public IQueryable<Agency> GetAgencies()
        {
            return db.Agencies;
        }

        // GET: api/Agencies/5
        [ResponseType(typeof(Agency))]
        public IHttpActionResult GetAgency(int id)
        {
            Agency agency = db.Agencies.Find(id);
            if (agency == null)
            {
                return NotFound();
            }

            return Ok(agency);
        }

        // PUT: api/Agencies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgency(int id, Agency agency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agency.AgencyKey)
            {
                return BadRequest();
            }

            db.Entry(agency).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyExists(id))
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

        // POST: api/Agencies
        [ResponseType(typeof(Agency))]
        public IHttpActionResult PostAgency(Agency agency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Agencies.Add(agency);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = agency.AgencyKey }, agency);
        }

        // DELETE: api/Agencies/5
        [ResponseType(typeof(Agency))]
        public IHttpActionResult DeleteAgency(int id)
        {
            Agency agency = db.Agencies.Find(id);
            if (agency == null)
            {
                return NotFound();
            }

            db.Agencies.Remove(agency);
            db.SaveChanges();

            return Ok(agency);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgencyExists(int id)
        {
            return db.Agencies.Count(e => e.AgencyKey == id) > 0;
        }
    }
}