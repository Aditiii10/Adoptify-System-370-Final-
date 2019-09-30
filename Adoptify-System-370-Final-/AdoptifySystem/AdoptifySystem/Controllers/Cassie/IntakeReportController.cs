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
{
    public class IntakeReportController : Controller
    {
        [HttpGet]
        // GET: IntakeReport
        public ActionResult Index()
        {
            IntakeReportVM vm = new IntakeReportVM();

            //Set default values for the FROM and TO dates
            vm.DateFrom = new DateTime(2019, 1, 1);
            vm.DateTo = new DateTime(2019, 1, 31);


            return View(vm);
        }
        private SelectList GetAnimals(int selected)
        {
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                var animals = db.Animals.Select(x => new SelectListItem
                {
                    Value = x.Animal_ID.ToString(),
                    Text = x.Animal_Name
                }).ToList();

                if (selected == 0)
                    return new SelectList(animals, "Value", "Text");
                else
                    return new SelectList(animals, "Value", "Text", selected);
            }
        }
        [HttpPost]
        public ActionResult View(IntakeReportVM vm)
        {
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;


                var list = db.Animals.Where(pp => pp.Animal_Entry_Date >= vm.DateFrom && pp.Animal_Entry_Date <= vm.DateTo).ToList().Select(rr => new ReportRecordAnimal
                {
                    Animal_Entry_Date = rr.Animal_Entry_Date.ToString(),
                    Animal_Name = rr.Animal_Name,
                    Animal_Type_Name = db.Animal_Type.Where(pp => pp.Animal_Type_ID == rr.Animal_Type_ID).Select(x => x.Animal_Type_Name).FirstOrDefault(),
                    Animal_Gender = rr.Animal_Gender,
                    Animal_Age = Convert.ToDouble(rr.Animal_Age),
                    Animal_Status_Name = db.Animal_Status.Where(pp => pp.Animal_Status_ID == rr.Animal_Status_ID).Select(x => x.Animal_Status_Name).FirstOrDefault(),
                });

                vm.results = list.GroupBy(g => g.Animal_Type_Name).ToList();

                vm.chartData = list.GroupBy(g => g.Animal_Type_Name).ToDictionary(g => g.Key, g => g.Sum(v => v.Animal_Age));


                TempData["chartData"] = vm.chartData;
                TempData["records"] = list.ToList();
                TempData["animal"] = vm.animal;
                return View("View", vm);
            }

        }

        public ActionResult IntakeReportChart()
        {
            var data = TempData["chartData"];

            return View(TempData["chartData"]);

        }

        private IntakeAnimalModel GetDataSet()
        {
            IntakeAnimalModel data = new IntakeAnimalModel();

            data.Animal.Clear();
            data.Animal.Rows.Clear();

            //Add table with details 
            DataRow vrow = data.Animal.NewRow();
            Animal animal = (Animal)TempData["animal"];
            vrow["ID"] = animal.Animal_ID;
            vrow["Name"] = animal.Animal_Name;
            vrow["Age"] = animal.Animal_Age;
            data.Animal.Rows.Add(vrow);

            //Add table  details  on Crystal Report
            foreach (var item in (IEnumerable<ReportRecordAnimal>)TempData["records"])
            {
                DataRow row = data.Animal.NewRow();
                row["EntryDate"] = item.Animal_Entry_Date;
                row["Animal_Age"] = item.Animal_Age;
                row["Animal_Gender"] = item.Animal_Gender;
                row["Animal_Name"] = item.Animal_Name;
                row["AnimalID"] = item.Animal_ID;
                data.Animal.Rows.Add(row);
            }



            TempData["chartData"] = TempData["chartData"];
            TempData["records"] = TempData["records"];
            TempData["animal"] = TempData["animal"];
            return data;
        }
        public ActionResult ExportPDF()
        {
            ReportDocument report = new ReportDocument();
            report.Load(Path.Combine(Server.MapPath("~/CrystalReport/AnimalIntake.rpt")));
            report.SetDataSource(GetDataSet());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "IntakeReport.pdf");
        }
    }
}