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
    public class textResponse
    {
        public string text { get; set; }
    }

    public class SlackPost
    {
        public string token { get; set; }
        public string team_id { get; set; }
        public string team_domain { get; set; }
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string command { get; set; }
        public string text { get; set; }
    }

    public class ChangelogEntriesController : ApiController
    {
        private BulbDigitalChangelogContext db = new BulbDigitalChangelogContext();

        // GET: api/ChangelogEntries
        public IQueryable<ChangelogEntry> GetChangelogEntries()
        {
            return db.ChangelogEntries;
        }

        [Route("api/ChangelogEntries/Slack")]
        public textResponse GetChangelogEntriesForSlack()
        {
            string result = "";
            var releaseGroups = db.Releases.GroupBy(r => r.FrameworkKey, r => r, (key, r) => new { FrameworkKey = key, CurrentReleaseKey = r.OrderByDescending(e => e.Version).Select(e => e.ReleaseKey).FirstOrDefault() });
            List<int> currentReleaseKeys = releaseGroups.Select(rg => rg.CurrentReleaseKey).ToList();
            var groups = db.ChangelogEntries.Include("Framework").Where(c => currentReleaseKeys.Contains(c.ReleaseKey)).GroupBy(c => c.Framework.Name, c=> c, (key, c) => new { FrameworkName = key, Changelogs = c.ToList()}).ToList();

            foreach (var group in groups)
            {
                result += "*" + group.FrameworkName + "*\n";
                foreach(ChangelogEntry changelog in group.Changelogs)
                {
                    result += changelog.Description + "\n";
                }
                result += "\n";
            }

            if(result == "")
            {
                result = "No Pending Updates";
            }

            textResponse res = new textResponse() { text = result };
            return res;
        }

        // GET: api/ChangelogEntries/5
        [ResponseType(typeof(ChangelogEntry))]
        public IHttpActionResult GetChangelogEntry(int id)
        {
            ChangelogEntry changelogEntry = db.ChangelogEntries.Find(id);
            if (changelogEntry == null)
            {
                return NotFound();
            }

            return Ok(changelogEntry);
        }

        // PUT: api/ChangelogEntries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChangelogEntry(int id, ChangelogEntry changelogEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != changelogEntry.ChangelogEntryKey)
            {
                return BadRequest();
            }

            db.Entry(changelogEntry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChangelogEntryExists(id))
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

        [HttpPost]
        [Route("api/ChangelogEntries/{framework}/SlackPost", Name = "PostChangelogEntryFromSlack")]
        [ResponseType(typeof(string))]
        public IHttpActionResult PostChangelogEntryFromSlack([FromUri()] string framework, [FromBody()] SlackPost slackPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string returnString = "";

            if(slackPost.text != null)
            {
                Framework fw = db.Frameworks.Where(f => f.Name.ToLower() == framework.ToLower()).FirstOrDefault();
                if(fw != null)
                {
                    Release currentRelease = db.Releases.Where(r => r.FrameworkKey == fw.FrameworkKey).OrderByDescending(r => r.Version).FirstOrDefault();
                    if (currentRelease != null)
                    {
                        ChangelogEntry newEntry = new ChangelogEntry()
                        {
                            CreatedByUser = slackPost.user_id,
                            DateLogged = DateTime.Now,
                            Description = slackPost.text,
                            FrameworkKey = fw.FrameworkKey,
                            ReleaseKey = currentRelease.ReleaseKey
                        };
                        db.ChangelogEntries.Add(newEntry);
                        db.SaveChanges();
                    }
                }
                returnString = "*Added to " + framework + " changelog*: " + slackPost.text;
            }
            else
            {
                returnString = "You have to enter a description to add a changelog";
            }

            var res = new { text = returnString };

            return CreatedAtRoute("PostChangelogEntryFromSlack", new { id = 1 }, res);
        }

        [HttpPost]
        [Route("api/Info/Slack", Name = "GetSlackInfo")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetSlackInfo(SlackPost slackPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //changelogEntry.DateLogged = DateTime.Now;
            //db.ChangelogEntries.Add(changelogEntry);
            //db.SaveChanges();

            string toReturn = "";
            toReturn += slackPost.channel_id + ", ";
            toReturn += slackPost.channel_name + ", ";
            toReturn += slackPost.command + ", ";
            toReturn += slackPost.team_domain + ", ";
            toReturn += slackPost.team_id + ", ";
            toReturn += slackPost.text + ", ";
            toReturn += slackPost.token + ", ";
            toReturn += slackPost.user_id + ", ";
            toReturn += slackPost.user_name + ", ";

            return CreatedAtRoute("PostChangelogEntryFromSlack", new { id = 1 }, toReturn);
        }

        // POST: api/ChangelogEntries
        [ResponseType(typeof(ChangelogEntry))]
        public IHttpActionResult PostChangelogEntry(ChangelogEntry changelogEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            changelogEntry.DateLogged = DateTime.Now;
            db.ChangelogEntries.Add(changelogEntry);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = changelogEntry.ChangelogEntryKey }, changelogEntry);
        }

        // DELETE: api/ChangelogEntries/5
        [ResponseType(typeof(ChangelogEntry))]
        public IHttpActionResult DeleteChangelogEntry(int id)
        {
            ChangelogEntry changelogEntry = db.ChangelogEntries.Find(id);
            if (changelogEntry == null)
            {
                return NotFound();
            }

            db.ChangelogEntries.Remove(changelogEntry);
            db.SaveChanges();

            return Ok(changelogEntry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChangelogEntryExists(int id)
        {
            return db.ChangelogEntries.Count(e => e.ChangelogEntryKey == id) > 0;
        }
    }
}