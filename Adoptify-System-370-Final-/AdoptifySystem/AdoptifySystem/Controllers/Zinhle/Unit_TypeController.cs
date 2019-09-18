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
    public class Unit_TypeController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Unit_Type
        public ActionResult Index()
        {
            return View(db.Unit_Type.ToList());
        }

        // GET: Unit_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_Type unit_Type = db.Unit_Type.Find(id);
            if (unit_Type == null)
            {
                return HttpNotFound();
            }
            return View(unit_Type);
        }

        // GET: Unit_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Unit_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Unit_Type_ID,Unit_Type_Name")] Unit_Type unit_Type)
        {
            if (ModelState.IsValid)
            {
                db.Unit_Type.Add(unit_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unit_Type);
        }

        // GET: Unit_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_Type unit_Type = db.Unit_Type.Find(id);
            if (unit_Type == null)
            {
                return HttpNotFound();
            }
            return View(unit_Type);
        }

        // POST: Unit_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Unit_Type_ID,Unit_Type_Name")] Unit_Type unit_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unit_Type);
        }

        // GET: Unit_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_Type unit_Type = db.Unit_Type.Find(id);
            if (unit_Type == null)
            {
                return HttpNotFound();
            }
            return View(unit_Type);
        }

        // POST: Unit_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit_Type unit_Type = db.Unit_Type.Find(id);
            db.Unit_Type.Remove(unit_Type);
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
