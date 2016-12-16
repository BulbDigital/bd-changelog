using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BulbDigitalChangelog.Models;

namespace BulbDigitalChangelog.Controllers
{
    public class HomeController : Controller
    {
        private BulbDigitalChangelogContext db = new BulbDigitalChangelogContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.ChangelogEntries.ToList());
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangelogEntry changelogEntry = db.ChangelogEntries.Find(id);
            if (changelogEntry == null)
            {
                return HttpNotFound();
            }
            return View(changelogEntry);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChangelogEntryKey,ProjectKey,Description,DateLogged,HasBeenProvisioned")] ChangelogEntry changelogEntry)
        {
            if (ModelState.IsValid)
            {
                db.ChangelogEntries.Add(changelogEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(changelogEntry);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangelogEntry changelogEntry = db.ChangelogEntries.Find(id);
            if (changelogEntry == null)
            {
                return HttpNotFound();
            }
            return View(changelogEntry);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChangelogEntryKey,ProjectKey,Description,DateLogged,HasBeenProvisioned")] ChangelogEntry changelogEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(changelogEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(changelogEntry);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangelogEntry changelogEntry = db.ChangelogEntries.Find(id);
            if (changelogEntry == null)
            {
                return HttpNotFound();
            }
            return View(changelogEntry);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChangelogEntry changelogEntry = db.ChangelogEntries.Find(id);
            db.ChangelogEntries.Remove(changelogEntry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
