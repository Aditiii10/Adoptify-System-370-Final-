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
    public class Packaging_TypeController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Packaging_Type
        public ActionResult Index()
        {
            return View(db.Packaging_Type.ToList());
        }

        // GET: Packaging_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packaging_Type packaging_Type = db.Packaging_Type.Find(id);
            if (packaging_Type == null)
            {
                return HttpNotFound();
            }
            return View(packaging_Type);
        }

        // GET: Packaging_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Packaging_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Packaging_Type_ID,Packaging_Type_Name")] Packaging_Type packaging_Type)
        {
            if (ModelState.IsValid)
            {
                db.Packaging_Type.Add(packaging_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packaging_Type);
        }

        // GET: Packaging_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packaging_Type packaging_Type = db.Packaging_Type.Find(id);
            if (packaging_Type == null)
            {
                return HttpNotFound();
            }
            return View(packaging_Type);
        }

        // POST: Packaging_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Packaging_Type_ID,Packaging_Type_Name")] Packaging_Type packaging_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packaging_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packaging_Type);
        }

        // GET: Packaging_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packaging_Type packaging_Type = db.Packaging_Type.Find(id);
            if (packaging_Type == null)
            {
                return HttpNotFound();
            }
            return View(packaging_Type);
        }

        // POST: Packaging_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Packaging_Type packaging_Type = db.Packaging_Type.Find(id);
            db.Packaging_Type.Remove(packaging_Type);
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
