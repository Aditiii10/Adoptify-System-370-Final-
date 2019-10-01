using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class Audit_LogController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Audit_Log
        public ActionResult Index()
        {
            var audit_Log = db.Audit_Log.Include(a => a.User_);
            return View(audit_Log.ToList());
        }

        // GET: Audit_Log/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit_Log audit_Log = db.Audit_Log.Find(id);
            if (audit_Log == null)
            {
                return HttpNotFound();
            }
            return View(audit_Log);
        }

        // GET: Audit_Log/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User_, "UserID", "Username");
            return View();
        }

        // POST: Audit_Log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Auditlog_ID,Auditlog_DateTime,Transaction_Type,Critical_Date,UserID")] Audit_Log audit_Log)
        {
            if (ModelState.IsValid)
            {
                db.Audit_Log.Add(audit_Log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User_, "UserID", "Username", audit_Log.UserID);
            return View(audit_Log);
        }

        // GET: Audit_Log/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit_Log audit_Log = db.Audit_Log.Find(id);
            if (audit_Log == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User_, "UserID", "Username", audit_Log.UserID);
            return View(audit_Log);
        }

        // POST: Audit_Log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Auditlog_ID,Auditlog_DateTime,Transaction_Type,Critical_Date,UserID")] Audit_Log audit_Log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audit_Log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User_, "UserID", "Username", audit_Log.UserID);
            return View(audit_Log);
        }

        // GET: Audit_Log/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audit_Log audit_Log = db.Audit_Log.Find(id);
            if (audit_Log == null)
            {
                return HttpNotFound();
            }
            return View(audit_Log);
        }

        // POST: Audit_Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Audit_Log audit_Log = db.Audit_Log.Find(id);
            db.Audit_Log.Remove(audit_Log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string search,string[] option)
        {
            List<Audit_Log> list = new List<Audit_Log>();

            foreach (var item in option)
            {
                List<Audit_Log> templist = new List<Audit_Log>();
                switch (item)
                {
                    case "Transaction Type":
                       
                            templist = db.Audit_Log.Where(z => z.Transaction_Type.StartsWith(search)).ToList();
                        if (list.Count>0)
                        {
                            int count = 0;
                            foreach(var audit in templist)
                            {
                               foreach(var cur in list)
                                {
                                    
                                    if (audit.Auditlog_ID == cur.Auditlog_ID)
                                    {
                                        count++;
                                    }
                                }
                                if (count == 0)
                                {
                                    list.Add(audit);
                                }
                            }
                        }
                        
                        
                    break;
                    case "User":
                        list = db.Audit_Log.Where(z => z.User_.Employee.Emp_Name.StartsWith(search)).ToList();
                        if (list.Count > 0)
                        {
                            int count = 0;
                            foreach (var audit in templist)
                            {
                                foreach (var cur in list)
                                {

                                    if (audit.Auditlog_ID == cur.Auditlog_ID)
                                    {
                                        count++;
                                    }
                                }
                                if (count == 0)
                                {
                                    list.Add(audit);
                                }
                            }
                        }
                        break;
                    case "Date":
                        list = db.Audit_Log.Where(z => z.Critical_Date.StartsWith(search)).ToList();
                        if (list.Count > 0)
                        {
                            int count = 0;
                            foreach (var audit in templist)
                            {
                                foreach (var cur in list)
                                {

                                    if (audit.Auditlog_ID == cur.Auditlog_ID)
                                    {
                                        count++;
                                    }
                                }
                                if (count == 0)
                                {
                                    list.Add(audit);
                                }
                            }
                        }
                        break;
                }
            }
            return View("Index",list);
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
