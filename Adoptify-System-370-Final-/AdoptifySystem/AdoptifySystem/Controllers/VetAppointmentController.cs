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
    public class VetAppointmentController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: VetAppointment
        public ActionResult Index()
        {
            var vet_Appointment_Master = db.Vet_Appointment_Master.Include(v => v.Animal).Include(v => v.Veterinarian).Include(v => v.VetAppReason);
            return View(vet_Appointment_Master.ToList());
        }

        public ActionResult SearchVetApp()
        {
            ViewBag.errormessage = "";
            List<Vet_Appointment_Master> vetApp = new List<Vet_Appointment_Master>();
            try
            {
                vetApp = db.Vet_Appointment_Master.ToList();
                if (vetApp.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(vetApp);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "Sorry! There was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
        }

        [HttpGet]
        public ActionResult SearchVetApp(string search)
        {
            try
            {
                if (search != null)
                {

                    List<Vet_Appointment_Master> Adopt = new List<Vet_Appointment_Master>();
                    try
                    {

                        Adopt = db.Vet_Appointment_Master.Where(z => z.Veterinarian.Vet_Address.StartsWith(search) || z.Veterinarian.Vet_Name.StartsWith(search) || z.Animal.Animal_Name.StartsWith(search)).ToList();
                        if (Adopt.Count == 0)
                        {
                            ViewBag.err = "No results found";
                            return View("SearchVetApp", Adopt);
                        }
                        return View("SearchVetApp", Adopt);
                    }
                    catch (Exception e)
                    {
                        ViewBag.err = "there was a network error: " + e.Message;
                        throw new Exception("Something Went Wrong!");
                    }
                }
                else
                {

                }

                return View();

            }
            catch (Exception)
            {

                throw new Exception("Something is wrong");
            }
        }


        // GET: VetAppointment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Vet_Appointment_Master vet_Appointment_Master = db.Vet_Appointment_Master.Find(id);
            if (vet_Appointment_Master == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(vet_Appointment_Master);
        }

        // GET: VetAppointment/Create
        public ActionResult Create(DateTime? date)
        {
            List<Animal> animals = db.Animals.ToList();
            List<Animal> animals2 = new List<Animal>();
            foreach (Animal item in animals)
            {
                if (item.Animal_Status_ID == 2)
                    animals2.Add(item);
            }
            ViewBag.Animal_ID = new SelectList(animals2, "Animal_ID", "Animal_Name");
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name");
            ViewBag.VetAppReasonsID = new SelectList(db.VetAppReasons, "VetAppReasonsID", "Reason");
            return View();
        }

        // POST: VetAppointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vet_Appoint_Line_ID,Vet_ID,AppointmentDate,Description,Animal_ID,VetAppReasonsID")] Vet_Appointment_Master vet_Appointment_Master, DateTime? date)
        {
            if (ModelState.IsValid)
            {
                vet_Appointment_Master.AppointmentDate = date.Value;
                db.Vet_Appointment_Master.Add(vet_Appointment_Master);
                TempData["SuccessMessage"] = "Created Veternarian Appointment Successfully";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Animal> animals = db.Animals.ToList();
            List<Animal> animals2 = new List<Animal>();
            foreach (Animal item in animals)
            {
                if(item.Animal_Status_ID==2)
                animals2.Add(item);
            }
            ViewBag.Animal_ID = new SelectList(animals2, "Animal_ID", "Animal_Name", vet_Appointment_Master.Animal_ID);
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name", vet_Appointment_Master.Vet_ID);
            ViewBag.VetAppReasonsID = new SelectList(db.VetAppReasons, "VetAppReasonsID", "Reason", vet_Appointment_Master.VetAppReasonsID);
            return View(vet_Appointment_Master);
        }

        // GET: VetAppointment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Vet_Appointment_Master vet_Appointment_Master = db.Vet_Appointment_Master.Find(id);
            if (vet_Appointment_Master == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            ViewBag.Animal_ID = new SelectList(db.Animals, "Animal_ID", "Animal_Name", vet_Appointment_Master.Animal_ID);
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name", vet_Appointment_Master.Vet_ID);
            ViewBag.VetAppReasonsID = new SelectList(db.VetAppReasons, "VetAppReasonsID", "Reason", vet_Appointment_Master.VetAppReasonsID);
            return View(vet_Appointment_Master);
        }

        // POST: VetAppointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vet_Appoint_Line_ID,Vet_ID,AppointmentDate,Description,Animal_ID,VetAppReasonsID")] Vet_Appointment_Master vet_Appointment_Master, DateTime? date)
        {
            if (ModelState.IsValid)
            {
                vet_Appointment_Master.AppointmentDate = date.Value;
                db.Entry(vet_Appointment_Master).State = EntityState.Modified;
                db.SaveChanges();
                TempData["EditMessage"] = "Updated Veternarian Appointment Successfully";
                return RedirectToAction("Index");
            }
            ViewBag.Animal_ID = new SelectList(db.Animals, "Animal_ID", "Animal_Name", vet_Appointment_Master.Animal_ID);
            ViewBag.Vet_ID = new SelectList(db.Veterinarians, "Vet_ID", "Vet_Name", vet_Appointment_Master.Vet_ID);
            ViewBag.VetAppReasonsID = new SelectList(db.VetAppReasons, "VetAppReasonsID", "Reason", vet_Appointment_Master.VetAppReasonsID);
            return View(vet_Appointment_Master);
        }

        // GET: VetAppointment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Vet_Appointment_Master vet_Appointment_Master = db.Vet_Appointment_Master.Find(id);
            if (vet_Appointment_Master == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(vet_Appointment_Master);
        }

        // POST: VetAppointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                Vet_Appointment_Master vet_Appointment_Master = db.Vet_Appointment_Master.Find(id);
                int count = vet_Appointment_Master.Mecidal_Card.Count();
                if (count != 0)
                {
                    TempData["DeleteErrorMessage"] = "You can not delete this Item as it is been used else where!!";
                    return RedirectToAction("Index");
                }

                else
                {
                    db.Vet_Appointment_Master.Remove(vet_Appointment_Master);
                    db.SaveChanges();
                    TempData["DeleteMessage"] = "Deleted Veternarian Appointment Successfully";
                    return RedirectToAction("Index");
                }
            }
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
