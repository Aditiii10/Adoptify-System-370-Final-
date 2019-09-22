using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;

namespace AdoptifySystem.Controllers
{
    public class Mecidal_CardController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Mecidal_Card
        public ActionResult Index()
        {
            var mecidal_Card = db.Mecidal_Card.Include(m => m.Animal).Include(m => m.Vet_Appointment_Master);
            return View(mecidal_Card.ToList());
        }

        // GET: Mecidal_Card/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mecidal_Card mecidal_Card = db.Mecidal_Card.Find(id);
            if (mecidal_Card == null)
            {
                return HttpNotFound();
            }
            return View(mecidal_Card);
        }

        // GET: Mecidal_Card/Create
        public ActionResult Create()
        {
            ViewBag.Animal_ID = new SelectList(db.Animals, "Animal_ID", "Animal_Name");
            ViewBag.Vet_Appoint_Line_ID = new SelectList(db.Vet_Appointment_Master, "Vet_Appoint_Line_ID", "Description");
            return View();
        }

        // POST: Mecidal_Card/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Animal_ID,Vet_Appointment_ID,Diagnosis_Details,Arrival_from_Vet,Next_Appointment_Date_,MedicalCard,Animal_Treatment,Vet_Appoint_Line_ID")] Mecidal_Card mecidal_Card, DateTime? date)
        {
            if (ModelState.IsValid)
            {
                mecidal_Card.Arrival_from_Vet = date.Value;
                mecidal_Card.Next_Appointment_Date_ = date.Value;
                db.Mecidal_Card.Add(mecidal_Card);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Saved New Medical Card Successfully";
                return RedirectToAction("Index");
            }

            ViewBag.Animal_ID = new SelectList(db.Animals, "Animal_ID", "Animal_Name", mecidal_Card.Animal_ID);
            ViewBag.Vet_Appoint_Line_ID = new SelectList(db.Vet_Appointment_Master, "Vet_Appoint_Line_ID", "Description", mecidal_Card.Vet_Appoint_Line_ID);
            return View(mecidal_Card);
        }

        // GET: Mecidal_Card/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mecidal_Card mecidal_Card = db.Mecidal_Card.Find(id);
            if (mecidal_Card == null)
            {
                return HttpNotFound();
            }
            ViewBag.Animal_ID = new SelectList(db.Animals, "Animal_ID", "Animal_Name", mecidal_Card.Animal_ID);
            ViewBag.Vet_Appoint_Line_ID = new SelectList(db.Vet_Appointment_Master, "Vet_Appoint_Line_ID", "Description", mecidal_Card.Vet_Appoint_Line_ID);
            return View(mecidal_Card);
        }

        // POST: Mecidal_Card/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Animal_ID,Vet_Appointment_ID,Diagnosis_Details,Arrival_from_Vet,Next_Appointment_Date_,MedicalCard,Animal_Treatment,Vet_Appoint_Line_ID")] Mecidal_Card mecidal_Card, DateTime? date)
        {
            if (ModelState.IsValid)
            {
                mecidal_Card.Arrival_from_Vet = date.Value;
                mecidal_Card.Next_Appointment_Date_ = date.Value;
                db.Entry(mecidal_Card).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditMessage"] = "Updated Medical Card Details Successfully";
                return RedirectToAction("Index");
            }
            ViewBag.Animal_ID = new SelectList(db.Animals, "Animal_ID", "Animal_Name", mecidal_Card.Animal_ID);
            ViewBag.Vet_Appoint_Line_ID = new SelectList(db.Vet_Appointment_Master, "Vet_Appoint_Line_ID", "Description", mecidal_Card.Vet_Appoint_Line_ID);
            return View(mecidal_Card);
        }

        // GET: Mecidal_Card/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mecidal_Card mecidal_Card = db.Mecidal_Card.Find(id);
            if (mecidal_Card == null)
            {
                return HttpNotFound();
            }
            return View(mecidal_Card);
        }

        // POST: Mecidal_Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mecidal_Card mecidal_Card = db.Mecidal_Card.Find(id);
            db.Mecidal_Card.Remove(mecidal_Card);
            db.SaveChanges();
            TempData["DeleteMessage"] = "Deleted Veternarian Successfully";
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
