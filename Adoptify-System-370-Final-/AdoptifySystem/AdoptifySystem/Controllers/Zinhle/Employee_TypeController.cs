using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class Employee_TypeController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Employee_Type
        public ActionResult Index()
        {
            return View(db.Employee_Type.ToList());
        }

        // GET: Employee_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_Type employee_Type = db.Employee_Type.Find(id);
            if (employee_Type == null)
            {
                return HttpNotFound();
            }
            return View(employee_Type);
        }

        // GET: Employee_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Emp_Type_ID,Emp_Type_Name,Emp_Type_Description")] Employee_Type employee_Type)
        {
            if (ModelState.IsValid)
            {
                db.Employee_Type.Add(employee_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee_Type);
        }

        // GET: Employee_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_Type employee_Type = db.Employee_Type.Find(id);
            if (employee_Type == null)
            {
                return HttpNotFound();
            }
            return View(employee_Type);
        }

        // POST: Employee_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Emp_Type_ID,Emp_Type_Name,Emp_Type_Description")] Employee_Type employee_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee_Type);
        }

        // GET: Employee_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_Type employee_Type = db.Employee_Type.Find(id);
            if (employee_Type == null)
            {
                return HttpNotFound();
            }
            return View(employee_Type);
        }

        // POST: Employee_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee_Type employee_Type = db.Employee_Type.Find(id);
            db.Employee_Type.Remove(employee_Type);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SearchEmployeeType()
        {
            ViewBag.errormessage = "";
            List<Employee_Type> employee_Types = new List<Employee_Type>();
            try
            {
                employee_Types = db.Employee_Type.ToList();
                if (employee_Types.Count == 0)
                {

                }
                return View(employee_Types);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
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
