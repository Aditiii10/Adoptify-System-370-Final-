using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;
using AdoptifySystem.Models;

namespace AdoptifySystem.Controllers
{
    public class Vet_AppointmentController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Vet_Appointment
        public ActionResult Index()
        {
            var vet_Appointment = db.Vet_Appointment.Include(v => v.Veterinarian);
            return View(vet_Appointment.ToList());
        }

        // GET: Vet_Appointment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vet_Appointment vet_Appointment = db.Vet_Appointment.Find(id);
            if (vet_Appointment == null)
            {
                return HttpNotFound();
            }
            return View(vet_Appointment);
        }

        // GET: Vet_Appointment/Create
        public ActionResult Create()
        {
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name");
            return View();
        }

        // POST: Vet_Appointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vet_Appointment_ID,Vet_ID")] Vet_Appointment vet_Appointment)
        {
            if (ModelState.IsValid)
            {
                db.Vet_Appointment.Add(vet_Appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name", vet_Appointment.Vet_ID);
            return View(vet_Appointment);
        }

        // GET: Vet_Appointment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vet_Appointment vet_Appointment = db.Vet_Appointment.Find(id);
            if (vet_Appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name", vet_Appointment.Vet_ID);
            return View(vet_Appointment);
        }

        // POST: Vet_Appointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vet_Appointment_ID,Vet_ID")] Vet_Appointment vet_Appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vet_Appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name", vet_Appointment.Vet_ID);
            return View(vet_Appointment);
        }

        // GET: Vet_Appointment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vet_Appointment vet_Appointment = db.Vet_Appointment.Find(id);
            if (vet_Appointment == null)
            {
                return HttpNotFound();
            }
            return View(vet_Appointment);
        }

        // POST: Vet_Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vet_Appointment vet_Appointment = db.Vet_Appointment.Find(id);
            db.Vet_Appointment.Remove(vet_Appointment);
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
