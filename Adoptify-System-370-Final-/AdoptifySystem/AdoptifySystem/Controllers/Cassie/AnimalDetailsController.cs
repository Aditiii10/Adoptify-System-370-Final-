using AdoptifySystem.CrystalReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Cassie
{
    public class AnimalDetailsController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: AnimalDetails
        public ActionResult Index()
        {
            return View(db.Animals.ToList());
        }

        public ActionResult ExportPDF()
        {
            ReportDocument report = new ReportDocument();
            report.Load(Path.Combine(Server.MapPath("~/CrystalReport/AnimalDetails.rpt")));
            report.SetDataSource(GetData());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "AnimalDetails.pdf");
        }
        private AnimalDetailsModel GetData()
        {
            AnimalDetailsModel data = new AnimalDetailsModel();

            data.Animal.Clear();

            //Add table to dataset for general details to be shown on Crystal Report
            DataRow vrow = data.Animal.NewRow();
            Animal animal = (Animal)TempData["AuditTrail"];
            vrow["ID"] = animal.Animal_ID;
            vrow["Animal Name"] = animal.Transaction_Type;
            vrow["Critical Date"] = animal.Critical_Date;
            vrow["TUserID"] = animal.UserID;
            data.Animal.Rows.Add(vrow);
            return data;
        }

    }
}