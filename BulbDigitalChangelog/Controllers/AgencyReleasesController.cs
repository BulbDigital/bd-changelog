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
    public class AgencyReleasesController : ApiController
    {
        public class agencyProvision
        {
            public string agencyName;
            public string frameworkName;
        }
        private BulbDigitalChangelogContext db = new BulbDigitalChangelogContext();

        // GET: api/AgencyReleases
        public IQueryable<AgencyRelease> GetAgencyReleases()
        {
            return db.AgencyReleases.Include("Agency").Include("Release.Framework");
        }

        // GET: api/AgencyReleases/5
        [ResponseType(typeof(AgencyRelease))]
        public IHttpActionResult GetAgencyRelease(int id)
        {
            AgencyRelease agencyRelease = db.AgencyReleases.Find(id);
            if (agencyRelease == null)
            {
                return NotFound();
            }

            return Ok(agencyRelease);
        }

        [HttpPost]
        [Route("api/AgencyReleases/Provision", Name = "ProvisionToAgency")]
        [ResponseType(typeof(string))]
        public IHttpActionResult ProvisionToAgency(SlackPost sp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var spArray = sp.text.Split(null);
            agencyProvision agencyProvision = new agencyProvision() { agencyName = spArray[0], frameworkName = spArray[1] };

            string returnString = "";

            if (agencyProvision != null)
            {
                Agency agencyToProvision = db.Agencies.Where(a => a.Name.ToLower() == agencyProvision.agencyName.ToLower()).FirstOrDefault();
                Framework framework = db.Frameworks.Where(a => a.Name.ToLower() == agencyProvision.frameworkName.ToLower()).FirstOrDefault();
                if (agencyToProvision != null)
                {
                    AgencyRelease mostRecentAgencyRelease = db.AgencyReleases.Include("Release").Where(a => a.AgencyKey == agencyToProvision.AgencyKey && a.Release.FrameworkKey == framework.FrameworkKey).OrderByDescending(a => a.Release
                    .Version).FirstOrDefault();

                    List<Release> releasesToProvision = new List<Release>();
                    if (mostRecentAgencyRelease != null)
                    {
                        releasesToProvision = db.Releases.Where(r => r.FrameworkKey == framework.FrameworkKey && r.HasBeenPulled == true && r.Version > mostRecentAgencyRelease.Release.Version).ToList();
                    }
                    else
                    {
                        releasesToProvision = db.Releases.Where(r => r.FrameworkKey == framework.FrameworkKey && r.HasBeenPulled == true).ToList();
                    }

                    if (releasesToProvision.Count > 0)
                    {
                        foreach (Release release in releasesToProvision)
                        {
                            AgencyRelease ar = new AgencyRelease() { AgencyKey = agencyToProvision.AgencyKey, DateProvisioned = DateTime.Now, Provisioner = "Lil' Mike", ReleaseKey = release.ReleaseKey };
                            db.AgencyReleases.Add(ar);
                        }
                        db.SaveChanges();
                        returnString = "Provisioned " + agencyProvision.frameworkName + " to " + agencyProvision.agencyName;
                    }
                    else
                    {
                        returnString = "Could not find any releases to provision";
                    }
                    }
                else
                {
                    returnString = "Unable to match an agency with that name";
                }
            }
            else
            {
                returnString = "You have to enter a framework to build a release for";
            }

            var res = new { text = returnString };

            return CreatedAtRoute("ProvisionToAgency", new { id = 1 }, res);
        }

        // PUT: api/AgencyReleases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgencyRelease(int id, AgencyRelease agencyRelease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agencyRelease.AgencyReleaseKey)
            {
                return BadRequest();
            }

            db.Entry(agencyRelease).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyReleaseExists(id))
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

        // POST: api/AgencyReleases
        [ResponseType(typeof(AgencyRelease))]
        public IHttpActionResult PostAgencyRelease(AgencyRelease agencyRelease)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AgencyReleases.Add(agencyRelease);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = agencyRelease.AgencyReleaseKey }, agencyRelease);
        }

        // DELETE: api/AgencyReleases/5
        [ResponseType(typeof(AgencyRelease))]
        public IHttpActionResult DeleteAgencyRelease(int id)
        {
            AgencyRelease agencyRelease = db.AgencyReleases.Find(id);
            if (agencyRelease == null)
            {
                return NotFound();
            }

            db.AgencyReleases.Remove(agencyRelease);
            db.SaveChanges();

            return Ok(agencyRelease);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgencyReleaseExists(int id)
        {
            return db.AgencyReleases.Count(e => e.AgencyReleaseKey == id) > 0;
        }
    }
}