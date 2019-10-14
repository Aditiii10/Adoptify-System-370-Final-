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
    public class Adopter_ContractController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Adopter_Contract

        public ActionResult SearchAdopterContract()
        {
            ViewBag.errormessage = "";
            List<Adopter_Contract> AdopterContract = new List<Adopter_Contract>();
            try
            {
                AdopterContract = db.Adopter_Contract.ToList();
                if (AdopterContract.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(AdopterContract);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "Sorry! There was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpGet]
        public ActionResult SearchAdopterContract(string search)
        {
            if (search != null)
            {

                List<Adopter_Contract> AdopterForm = new List<Adopter_Contract>();
                try
                {

                    AdopterForm = db.Adopter_Contract.Where(z => z.Adopter.Adopter_Name.StartsWith(search)).ToList();
                    if (AdopterForm.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View("Index", AdopterForm);
                    }
                    return View("Index", AdopterForm);
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
            var adopter_Contract = db.Adopter_Contract.Include(a => a.Adopter);
            return View(adopter_Contract.ToList());
        }

        // GET: Adopter_Contract/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adopter_Contract adopter_Contract = db.Adopter_Contract.Find(id);
            if (adopter_Contract == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(adopter_Contract);
        }

        // GET: Adopter_Contract/Create
        public ActionResult Create()
        {
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name");
            return View();
        }

        // POST: Adopter_Contract/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Contract_ID,Electronic_Contract,Adopter_ID")] Adopter_Contract adopter_Contract)
        {
            if (ModelState.IsValid)
            {
                db.Adopter_Contract.Add(adopter_Contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adopter_Contract.Adopter_ID);
            return View(adopter_Contract);
        }

        // GET: Adopter_Contract/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adopter_Contract adopter_Contract = db.Adopter_Contract.Find(id);
            if (adopter_Contract == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adopter_Contract.Adopter_ID);
            return View(adopter_Contract);
        }

        // POST: Adopter_Contract/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Contract_ID,Electronic_Contract,Adopter_ID")] Adopter_Contract adopter_Contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adopter_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adopter_Contract.Adopter_ID);
            return View(adopter_Contract);
        }

        // GET: Adopter_Contract/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adopter_Contract adopter_Contract = db.Adopter_Contract.Find(id);
            if (adopter_Contract == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            return View(adopter_Contract);
        }

        // POST: Adopter_Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adopter_Contract adopter_Contract = db.Adopter_Contract.Find(id);
            db.Adopter_Contract.Remove(adopter_Contract);
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