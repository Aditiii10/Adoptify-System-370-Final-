using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class WolliesController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Wollies
        public ActionResult Index()
        {
            return View(db.Wollies.ToList());
        }

        // GET: Wollies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wolly wolly = db.Wollies.Find(id);
            if (wolly == null)
            {
                return HttpNotFound();
            }
            return View(wolly);
        }

        // GET: Wollies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wollies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Co_ID,Org_Name,Org_Address,Org_Type,Org_ContactNumber,Org_FaxNumber,Org_Email")] Wolly wolly)
        {
            if (ModelState.IsValid)
            {
                db.Wollies.Add(wolly);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wolly);
        }

        // GET: Wollies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wolly wolly = db.Wollies.Find(id);
            if (wolly == null)
            {
                return HttpNotFound();
            }
            return View(wolly);
        }

        // POST: Wollies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Co_ID,Org_Name,Org_Address,Org_Type,Org_ContactNumber,Org_FaxNumber,Org_Email")] Wolly wolly)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wolly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wolly);
        }

        // GET: Wollies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wolly wolly = db.Wollies.Find(id);
            if (wolly == null)
            {
                return HttpNotFound();
            }
            return View(wolly);
        }

        // POST: Wollies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wolly wolly = db.Wollies.Find(id);
            db.Wollies.Remove(wolly);
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
