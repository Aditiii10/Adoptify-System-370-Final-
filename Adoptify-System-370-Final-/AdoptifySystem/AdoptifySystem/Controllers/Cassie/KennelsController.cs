using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using OfficeOpenXml;

namespace AdoptifySystem.Controllers.Cassie
{
    public class KennelsController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Kennels
        public ActionResult Index()
        {
            List<KennelHistoryViewModel> kenlist = db.Animal_Kennel_History.Select(x => new KennelHistoryViewModel
            {
                Animal_Kennel_History_ID = x.Animal_Kennel_History_ID,
                Animal_ID = x.Animal_ID,
                Kennel_ID = x.Kennel_ID,
            }).ToList();


            return View(kenlist);
        }

        // GET: Kennels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kennel kennel = db.Kennels.Find(id);
            if (kennel == null)
            {
                return HttpNotFound();
            }
            return View(kennel);
        }

        // GET: Kennels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kennels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Kennel_ID,Kennel_Name,Kennel_Number,Kennel_Capacity")] Kennel kennel)
        {
            if (ModelState.IsValid)
            {
                db.Kennels.Add(kennel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kennel);
        }

        // GET: Kennels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kennel kennel = db.Kennels.Find(id);
            if (kennel == null)
            {
                return HttpNotFound();
            }
            return View(kennel);
        }

        // POST: Kennels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Kennel_ID,Kennel_Name,Kennel_Number,Kennel_Capacity")] Kennel kennel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kennel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kennel);
        }

        // GET: Kennels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kennel kennel = db.Kennels.Find(id);
            if (kennel == null)
            {
                return HttpNotFound();
            }
            return View(kennel);
        }

        // POST: Kennels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kennel kennel = db.Kennels.Find(id);
            db.Kennels.Remove(kennel);
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

        public void ExportToExcel()
        {
            List<KennelHistoryViewModel> kenlist = db.Animal_Kennel_History.Select(x => new KennelHistoryViewModel
            {
                Animal_Kennel_History_ID = x.Animal_Kennel_History_ID,
                Animal_ID = x.Animal_ID,
                Kennel_ID = x.Kennel_ID,
            }).ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Communication";
            ws.Cells["B1"].Value = "Com1";

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "Report1";

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Animal_Kennel_History_ID";
            ws.Cells["B6"].Value = "Animal_ID";
            ws.Cells["C6"].Value = "Kennel_ID";

            int rowStart = 7;
            foreach (var item in kenlist)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Animal_Kennel_History_ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Animal_ID;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Kennel_ID;
                rowStart++;


            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();


        }
    }
}

