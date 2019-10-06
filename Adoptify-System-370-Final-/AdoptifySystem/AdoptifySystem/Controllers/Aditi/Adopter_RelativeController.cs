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

namespace AdoptifySystem.Controllers.Aditi
{
    public class Adopter_RelativeController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Adopter_Relative

        public ActionResult SearchAdopterRelative()
        {
            ViewBag.errormessage = "";
            List<Adopter_Relative> AdopterRelative = new List<Adopter_Relative>();
            try
            {
                AdopterRelative = db.Adopter_Relative.ToList();
                if (AdopterRelative.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(AdopterRelative);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "Sorry! There was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpGet]
        public ActionResult SearchAdopterRelative(string search)
        {
            if (search != null)
            {

                List<Adopter_Relative> AdoptRelative = new List<Adopter_Relative>();
                try
                {

                    AdoptRelative = db.Adopter_Relative.Where(z => z.Relative_Name.StartsWith(search) || z.Relative_Surname.StartsWith(search) || z.Relative_Email.StartsWith(search) || z.Relative_Cell.StartsWith(search)).ToList(); 
                    if (AdoptRelative.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View("Index", AdoptRelative);
                    }
                    return View("Index", AdoptRelative);
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
        public ActionResult Index()
        {
            var adopter_Relative = db.Adopter_Relative.Include(a => a.Adopter);
            return View(adopter_Relative.ToList());
        }

        // GET: Adopter_Relative/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adopter_Relative adopter_Relative = db.Adopter_Relative.Find(id);
            if (adopter_Relative == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(adopter_Relative);
        }

        // GET: Adopter_Relative/Create
        public ActionResult Create()
        {
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name");
            return View();
        }

        // POST: Adopter_Relative/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Relative_ID,Relative_Name,Relative_Surname,Relative_Email,Relative_Address,Relative_PostalAddress,Relative_Home_Tel,Relative_Work_Tel,Relative_Cell,Relationship,Adopter_ID")] Adopter_Relative adopter_Relative)
        {
            if (ModelState.IsValid)
            {
                db.Adopter_Relative.Add(adopter_Relative);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adopter_Relative.Adopter_ID);
            return View(adopter_Relative);
        }

        // GET: Adopter_Relative/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adopter_Relative adopter_Relative = db.Adopter_Relative.Find(id);
            if (adopter_Relative == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adopter_Relative.Adopter_ID);
            return View(adopter_Relative);
        }

        // POST: Adopter_Relative/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Relative_ID,Relative_Name,Relative_Surname,Relative_Email,Relative_Address,Relative_PostalAddress,Relative_Home_Tel,Relative_Work_Tel,Relative_Cell,Relationship,Adopter_ID")] Adopter_Relative adopter_Relative)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adopter_Relative).State = EntityState.Modified;
                db.SaveChanges();

                //TempData["AdopterRelativeMessage"] = "Adopter Relative Successfully Created";
                return RedirectToAction("Index");
            }
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adopter_Relative.Adopter_ID);
            return View(adopter_Relative);
        }

        // GET: Adopter_Relative/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adopter_Relative adopter_Relative = db.Adopter_Relative.Find(id);
            if (adopter_Relative == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(adopter_Relative);
        }

        // POST: Adopter_Relative/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adopter_Relative adopter_Relative = db.Adopter_Relative.Find(id);
            db.Adopter_Relative.Remove(adopter_Relative);
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
