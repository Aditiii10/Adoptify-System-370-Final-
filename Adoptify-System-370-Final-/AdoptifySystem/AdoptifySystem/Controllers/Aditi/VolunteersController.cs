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
    public class VolunteersController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Volunteers

        public ActionResult SearchVolunteer()
        {
            ViewBag.errormessage = "";
            List<Volunteer> Volunteers = new List<Volunteer>();
            try
            {
                Volunteers = db.Volunteers.ToList();
                if (Volunteers.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(Volunteers);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "Sorry! There was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpPost]
        public ActionResult SearchVolunteer(string search)
        {
            try
            {
                if (search != null)
                {

                    List<Volunteer> Vol = new List<Volunteer>();
                    try
                    {
                        if (Vol.Count == 0)
                        {
                            ViewBag.err = "No results found";
                            return View(Vol);
                        }
                        return View(Vol);
                    }
                    catch (Exception e)
                    {
                        ViewBag.err = "Sorry! There was a network error: " + e.Message;
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
                throw new Exception("Something Went Wrong!");
            }

        }



        public ActionResult Index()
        { 
            var volunteers = db.Volunteers.Include(v => v.Title);
            return View(volunteers.ToList());
        }



        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(volunteer);
        }

        // GET: Volunteers/Create
        public ActionResult Create()
        {
            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vol_ID,Vol_Name,Vol_Surname,Vol_Email,Vol_ContactNumber,Vol_Address,Vol_Emergency_ContactName,Vol_Emergency_ContactNumber,Title_ID")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Volunteers.Add(volunteer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description", volunteer.Title_ID);
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description", volunteer.Title_ID);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vol_ID,Vol_Name,Vol_Surname,Vol_Email,Vol_ContactNumber,Vol_Address,Vol_Emergency_ContactName,Vol_Emergency_ContactNumber,Title_ID")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description", volunteer.Title_ID);
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            int count = volunteer.Volunteer_Hours.Count();
            if (count != 0)
            {
                TempData["DeleteErrorMessage"] = "You can not delete this item as it is been used else where!";
                return RedirectToAction("Index");
            }
            else
            {
                db.Volunteers.Remove(volunteer);
                db.SaveChanges();
                TempData["DeleteMessage"] = "Deleted Volunteer Successfully";
                return RedirectToAction("Index");
            }
            
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
