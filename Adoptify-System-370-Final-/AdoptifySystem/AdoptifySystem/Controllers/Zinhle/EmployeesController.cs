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
    public class EmployeesController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Title).Include(e => e.Employee_Type);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description");
            ViewBag.Emp_Type_ID = new SelectList(db.Employee_Type, "Emp_Type_ID", "Emp_Type_Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Emp_ID,Emp_Name,Emp_Surname,Emp_ContactNumber,Emp_IDNumber,Emp_Email,Emp_Contract,Emp_Date_Employed,Title_ID,Emp_Type_ID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description", employee.Title_ID);
            ViewBag.Emp_Type_ID = new SelectList(db.Employee_Type, "Emp_Type_ID", "Emp_Type_Name", employee.Emp_Type_ID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description", employee.Title_ID);
            ViewBag.Emp_Type_ID = new SelectList(db.Employee_Type, "Emp_Type_ID", "Emp_Type_Name", employee.Emp_Type_ID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Emp_ID,Emp_Name,Emp_Surname,Emp_ContactNumber,Emp_IDNumber,Emp_Email,Emp_Contract,Emp_Date_Employed,Title_ID,Emp_Type_ID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Title_ID = new SelectList(db.Titles, "Title_ID", "Title_Description", employee.Title_ID);
            ViewBag.Emp_Type_ID = new SelectList(db.Employee_Type, "Emp_Type_ID", "Emp_Type_Name", employee.Emp_Type_ID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
