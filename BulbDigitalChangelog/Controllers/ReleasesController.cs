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
    public class ReleasesController : ApiController
    {
        private BulbDigitalChangelogContext db = new BulbDigitalChangelogContext();

        // GET: api/Releases
        public IQueryable<Release> GetReleases()
        {
            return db.Releases;
        }

        // GET: api/Releases/5
        [ResponseType(typeof(Release))]
        public IHttpActionResult GetRelease(int id)
        {
            Release release = db.Releases.Find(id);
            if (release == null)
            {
                return NotFound();
            }

            return Ok(release);
        }

        [HttpPost]
        [Route("api/Releases/Build", Name = "BuildRelease")]
        [ResponseType(typeof(BuildResponse))]
        public IHttpActionResult BuildRelease(SlackPost slackPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string returnString = "";
            BuildResponse buildResponse = new BuildResponse();

            if (slackPost.text != null)
            {
                Framework fw = db.Frameworks.Where(f => f.Name.ToLower() == slackPost.text.ToLower()).FirstOrDefault();
                if (fw != null)
                {
                    Release release = db.Releases.Where(r => r.FrameworkKey == fw.FrameworkKey).OrderByDescending(r => r.Version).FirstOrDefault();
                    release.HasBeenPulled = true;
                    Release newRelease = new Release() { FrameworkKey = fw.FrameworkKey, Version = release.Version + 1, HasBeenPulled = false };
                    fw.Version = newRelease.Version;
                    db.Releases.Add(newRelease);
                    db.SaveChanges();

                    buildResponse.Framework = fw.Name;
                    buildResponse.BuildDate = DateTime.Now;
                    buildResponse.Version = release.Version.ToString();

                    List<ChangelogEntry> changesInRelease = db.ChangelogEntries.Where(ce => ce.ReleaseKey == release.ReleaseKey).ToList();

                    buildResponse.Changelogs = changesInRelease.Select(c => new ServiceChangelog() { DateLogged = c.DateLogged, ChangeNote = c.Description, Username = c.CreatedByUser }).ToList();

                    var ctl = new SlackWebhookController();

                    repoPost post = new repoPost()
                    {
                        type = "pull",
                        fallback = slackPost.user_name + " has pulled " + fw.Name,
                        message = "Pulled " + fw.Name,
                        framework = fw.Name,
                        username = slackPost.user_name
                    };

                    ctl.PostToSOMRepoActivity(post);
                    //returnString = "*Built release for " + fw.Name + "*";
                }
                else
                {
                    returnString = "Unable to match a framework with that name";
                }
            }
            else
            {
                returnString = "You have to enter a framework to build a release for";
            }

            //var res = new { text = returnString };

            return CreatedAtRoute("BuildRelease", new { id = 1 }, buildResponse);
        }

        // PUT: api/Releases/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRelease(int id, Release release)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != release.ReleaseKey)
            {
                return BadRequest();
            }

            db.Entry(release).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReleaseExists(id))
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

        // POST: api/Releases
        [ResponseType(typeof(Release))]
        public IHttpActionResult PostRelease(Release release)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Releases.Add(release);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = release.ReleaseKey }, release);
        }

        // DELETE: api/Releases/5
        [ResponseType(typeof(Release))]
        public IHttpActionResult DeleteRelease(int id)
        {
            Release release = db.Releases.Find(id);
            if (release == null)
            {
                return NotFound();
            }

            db.Releases.Remove(release);
            db.SaveChanges();

            return Ok(release);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReleaseExists(int id)
        {
            return db.Releases.Count(e => e.ReleaseKey == id) > 0;
        }

        //[HttpPost]
        //[Route("api/Releases/Agency", Name = "GetAgencyReleaseDetails")]
        //[ResponseType(typeof(string))]
        //public textResponse GetAgencyRelease(SlackPost sp)
        //{
        //    textResponse res = new textResponse();
        //    res.attachments = new List<Attachment>();

        //    int version = -1;
        //    if (Int32.TryParse(sp.text, out version))
        //    {
        //        Framework fw = db.Frameworks.Where(f => f.Name.ToLower() == "agency").FirstOrDefault();
        //        if (fw != null)
        //        {
        //            Release release = db.Releases.Where(r => r.FrameworkKey == fw.FrameworkKey && r.Version == version).FirstOrDefault();

        //            res.text = "Agency release version " + version + " contains:";

        //            List<ChangelogEntry> changesInRelease = db.ChangelogEntries.Where(ce => ce.ReleaseKey == release.ReleaseKey).ToList();

        //            foreach (ChangelogEntry entry in changesInRelease)
        //            {
        //                Attachment newAttachment = new Attachment() { fields = new List<Field>() };
        //                Field newField = new Field() { title = "", value = entry.Description };
        //                newAttachment.fields.Add(newField);
        //                res.attachments.Add(newAttachment);
        //            }

        //            if (res.attachments.Count == 0)
        //            {
        //                Attachment newAttachment = new Attachment() { fields = new List<Field>() };
        //                Field newField = new Field() { title = "", value = "No Changes Logged" };
        //                newAttachment.fields.Add(newField);
        //                res.attachments.Add(newAttachment);
        //            }
        //        }
        //        else
        //        {
        //            res.text = "Unable to find that agency release version";
        //        }
        //    }
        //    else
        //    {
        //        res.text = "Unable to find that agency release version";
        //    }


        //    return res;
            
        //}


        [HttpPost]
        [Route("api/Releases/{framework}", Name = "GetFrameworkReleaseDetails")]
        [ResponseType(typeof(string))]
        public textResponse GetCoreRelease([FromUri()] string framework, [FromBody()] SlackPost sp)
        {
            textResponse res = new textResponse();
            res.attachments = new List<Attachment>();

            int version = -1;
            if (Int32.TryParse(sp.text, out version))
            {
                Framework fw = db.Frameworks.Where(f => f.Name.ToLower() == framework.ToLower()).FirstOrDefault();
                if (fw != null)
                {
                    Release release = db.Releases.Where(r => r.FrameworkKey == fw.FrameworkKey && r.Version == version).FirstOrDefault();

                    if (release != null)
                    {
                        res.text = fw.Name + " release version " + version + " contains:";

                        List<ChangelogEntry> changesInRelease = db.ChangelogEntries.Where(ce => ce.ReleaseKey == release.ReleaseKey).ToList();

                        foreach (ChangelogEntry entry in changesInRelease)
                        {
                            Attachment newAttachment = new Attachment() { fields = new List<Field>() };
                            Field newField = new Field() { title = "", value = entry.Description };
                            newAttachment.fields.Add(newField);
                            res.attachments.Add(newAttachment);
                        }

                        if (res.attachments.Count == 0)
                        {
                            Attachment newAttachment = new Attachment() { fields = new List<Field>() };
                            Field newField = new Field() { title = "", value = "No Changes Logged" };
                            newAttachment.fields.Add(newField);
                            res.attachments.Add(newAttachment);
                        }
                    }
                    else
                    {
                        res.text = "Unable to find that release";
                    }
                }
                else
                {
                    res.text = "Unable to find that framework";
                }
            }
            else
            {
                res.text = "Unable to find that release version";
            }
            return res;

        }
    }
}