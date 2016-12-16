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
    public class FrameworksController : ApiController
    {
        private BulbDigitalChangelogContext db = new BulbDigitalChangelogContext();

        // GET: api/Frameworks
        public IQueryable<Framework> GetFrameworks()
        {
            return db.Frameworks;
        }

        // GET: api/Frameworks/5
        [ResponseType(typeof(Framework))]
        public IHttpActionResult GetFramework(int id)
        {
            Framework framework = db.Frameworks.Find(id);
            if (framework == null)
            {
                return NotFound();
            }

            return Ok(framework);
        }

        // PUT: api/Frameworks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFramework(int id, Framework framework)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != framework.FrameworkKey)
            {
                return BadRequest();
            }

            db.Entry(framework).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FrameworkExists(id))
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

        // POST: api/Frameworks
        [ResponseType(typeof(Framework))]
        public IHttpActionResult PostFramework(Framework framework)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Frameworks.Add(framework);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = framework.FrameworkKey }, framework);
        }

        // DELETE: api/Frameworks/5
        [ResponseType(typeof(Framework))]
        public IHttpActionResult DeleteFramework(int id)
        {
            Framework framework = db.Frameworks.Find(id);
            if (framework == null)
            {
                return NotFound();
            }

            db.Frameworks.Remove(framework);
            db.SaveChanges();

            return Ok(framework);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FrameworkExists(int id)
        {
            return db.Frameworks.Count(e => e.FrameworkKey == id) > 0;
        }
    }
}