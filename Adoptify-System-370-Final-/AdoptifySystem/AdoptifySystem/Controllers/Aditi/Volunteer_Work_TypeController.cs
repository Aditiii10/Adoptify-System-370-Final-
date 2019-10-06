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
    public class Volunteer_Work_TypeController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Volunteer_Work_Type
        public ActionResult SearchVolunteerWorkType()
        {
            ViewBag.errormessage = "";
            List<Volunteer_Work_Type> VolunteerWorkType = new List<Volunteer_Work_Type>();
            try
            {
                VolunteerWorkType = db.Volunteer_Work_Type.ToList();
                if (VolunteerWorkType.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(VolunteerWorkType);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "Sorry! There was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpGet]
        public ActionResult SearchVolunteerWorkType(string search)
        {
            if (search != null)
            {

                List<Volunteer_Work_Type> Vol_WT = new List<Volunteer_Work_Type>();
                try
                {

                    Vol_WT = db.Volunteer_Work_Type.Where(z => z.Vol_WorkType_Name.StartsWith(search)).ToList();
                    if (Vol_WT.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View("Index", Vol_WT);
                    }
                    return View("Index", Vol_WT);
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
            return View(db.Volunteer_Work_Type.ToList());
        }

        // GET: Volunteer_Work_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Volunteer_Work_Type volunteer_Work_Type = db.Volunteer_Work_Type.Find(id);
            if (volunteer_Work_Type == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(volunteer_Work_Type);
        }

        // GET: Volunteer_Work_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Volunteer_Work_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Vol_WorkType_ID,Vol_WorkType_Name,Vol_WorkType_Description")] Volunteer_Work_Type volunteer_Work_Type)
        {
            if (ModelState.IsValid)
            {
                db.Volunteer_Work_Type.Add(volunteer_Work_Type);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(volunteer_Work_Type);
        }

        // GET: Volunteer_Work_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Volunteer_Work_Type volunteer_Work_Type = db.Volunteer_Work_Type.Find(id);
            if (volunteer_Work_Type == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(volunteer_Work_Type);
        }

        // POST: Volunteer_Work_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vol_WorkType_ID,Vol_WorkType_Name,Vol_WorkType_Description")] Volunteer_Work_Type volunteer_Work_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer_Work_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(volunteer_Work_Type);
        }

        // GET: Volunteer_Work_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Volunteer_Work_Type volunteer_Work_Type = db.Volunteer_Work_Type.Find(id);
            if (volunteer_Work_Type == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(volunteer_Work_Type);
        }

        // POST: Volunteer_Work_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer_Work_Type volunteer_Work_Type = db.Volunteer_Work_Type.Find(id);
            db.Volunteer_Work_Type.Remove(volunteer_Work_Type);
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