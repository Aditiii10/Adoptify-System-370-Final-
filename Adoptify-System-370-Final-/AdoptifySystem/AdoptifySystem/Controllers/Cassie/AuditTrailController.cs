using AdoptifySystem.CrystalReport;
using AdoptifySystem.ViewModel;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Cassie
{ }
//{
//    public class AuditTrailController : Controller
//    {
//        [HttpGet]
//        // GET: AuditTrail
//        public ActionResult Index()
//        {
//            AuditTrailVM vm = new AuditTrailVM();

//            //DropDownList
//            vm.TransactionTypes = GetTransactionTypes(0);

//            //Default values 
//            vm.DateFrom = new DateTime(2019, 10, 1);
//            vm.DateTo = new DateTime(2019, 10, 31);


//            return View(vm);
//        }
//    }

//    private SelectList GetTransactionTypes(int selected)
//    {
//        using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
//        {
//            db.Configuration.ProxyCreationEnabled = false;

//            //SelectListItem for each TransactionType 
//            var transactiontypes = db.Audit_Log.Select(x => new SelectListItem
//            {
//                Value = x.Auditlog_ID.ToString(),
//                Text = x.Transaction_Type
//            }).ToList();

//            if (selected == 0)
//                return new SelectList(transactiontypes, "Value", "Text");
//            else
//                return new SelectList(transactiontypes, "Value", "Text", selected);
//        }
//    }
//    [HttpPost]
//    public ActionResult Index(AuditTrailVM vm)
//    {
//        using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
//        {
//            db.Configuration.ProxyCreationEnabled = false;

//            //Populate the dropdown 
//            vm.TransactionTypes = GetTransactionTypes(vm.SelectedTransactionTypeID);

//            vm.auditTrail = db.Audit_Log.Where(x => x.Auditlog_ID == vm.SelectedTransactionTypeID).FirstOrDefault();

//            var list = db.Audit_Log.Where(pp => pp.UserID == vm.user.UserID && pp.Auditlog_DateTime >= vm.DateFrom && pp.Auditlog_DateTime <= vm.DateTo).ToList().Select(rr => new ReportRecordAudit
//            {
//                Auditlog_DateTime = rr.Auditlog_DateTime.ToString("dd-mm-yyyy"),
//                Transaction_Type = rr.Transaction_Type,
//                Critical_Date = rr.Critical_Date,
//                Emp_Name = db.User_.Where(pp => pp.UserID == rr.UserID).Select(x => x.Username).FirstOrDefault()
//            });

//            vm.results = list.GroupBy(g => g.Transaction_Type).ToList();

//            return View(vm);
//        }

//    }
//    public ActionResult ExportPDF()
//    {
//        ReportDocument report = new ReportDocument();
//        report.Load(Path.Combine(Server.MapPath("~/CrystalReport/AuditTrail.rpt")));
//        report.SetDataSource(GetData());
//        Response.Buffer = false;
//        Response.ClearContent();
//        Response.ClearHeaders();
//        Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
//        stream.Seek(0, SeekOrigin.Begin);
//        return File(stream, "AuditTrail.pdf");
//    }

//    private AuditTrailModel GetData()
//    {
//        AuditTrailModel data = new AuditTrailModel();

//        data.Audit_Log.Clear();

//        //Add table to dataset for general details to be shown on Crystal Report
//        DataRow vrow = data.Audit_Log.NewRow();
//        Audit_Log auditTrail = (Audit_Log)TempData["AuditTrail"];
//        vrow["ID"] = auditTrail.Auditlog_ID;
//        vrow["Transaction Type"] = auditTrail.Transaction_Type;
//        vrow["Critical Date"] = auditTrail.Critical_Date;
//        vrow["TUserID"] = auditTrail.UserID;
//        data.Audit_Log.Rows.Add(vrow);
//        return data;
//    }


//}