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
    public class VeterinariansController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Veterinarians
        public ActionResult Index()
        {
            return View(db.Veterinarians.ToList());
        }

        // GET: Veterinarians/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinarian veterinarian = db.Veterinarians.Find(id);
            if (veterinarian == null)
            {
                return HttpNotFound();
            }
            return View(veterinarian);
        }

        // GET: Veterinarians/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Veterinarians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vet_ID,Vet_Name,Vet_Emial,Vet_Tel,Vet_Address")] Veterinarian veterinarian)
        {
            if (ModelState.IsValid)
            {
                db.Veterinarians.Add(veterinarian);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(veterinarian);
        }

        // GET: Veterinarians/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinarian veterinarian = db.Veterinarians.Find(id);
            if (veterinarian == null)
            {
                return HttpNotFound();
            }
            return View(veterinarian);
        }

        // POST: Veterinarians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vet_ID,Vet_Name,Vet_Emial,Vet_Tel,Vet_Address")] Veterinarian veterinarian)
        {
            if (ModelState.IsValid)
            {
                db.Entry(veterinarian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(veterinarian);
        }

        // GET: Veterinarians/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinarian veterinarian = db.Veterinarians.Find(id);
            if (veterinarian == null)
            {
                return HttpNotFound();
            }
            return View(veterinarian);
        }

        // POST: Veterinarians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)


        {
            Veterinarian veterinarian = db.Veterinarians.Find(id);
            db.Veterinarians.Remove(veterinarian);
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

        public ActionResult Search(string searchBy, string search)
        {
            if (searchBy == "Vet_Name")
            {
                return View(db.Veterinarians.Where(c => c.Vet_Name.Contains(search) || search == null).ToList());
            }
            else
            {
                return View(db.Veterinarians.Where(c => c.Vet_Address.Contains(search) || search == null).ToList());
            }

        }
    }
}
