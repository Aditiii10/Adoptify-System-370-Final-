using AdoptifySystem.CrystalReport;
using AdoptifySystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Cassie
{
    public class ComServeController : Controller
    {
        [HttpGet]
        // GET: ComServe
        public ActionResult Index()
        {
            ComServeVM vm = new ComServeVM();

            //Retrieve a list of vendors so that it can be used to populate the dropdown on the View
            vm.Volunteers = GetVolunteers(0);

            //Set default values for the FROM and TO dates
            vm.DateFrom = new DateTime(2019, 12, 1);
            vm.DateTo = new DateTime(2019, 12, 31);


            return View(vm);

        }

        private SelectList GetVolunteers(int selected)
        {
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                //SelectListItem for each Volunteer
                var volunteers = db.Volunteers.Select(x => new SelectListItem
                {
                    Value = x.Vol_ID.ToString(),
                    Text = x.Vol_Name
                }).ToList();

                if (selected == 0)
                    return new SelectList(volunteers, "Value", "Text");
                else
                    return new SelectList(volunteers, "Value", "Text", selected);
            }
        }

        [HttpPost]
        public ActionResult Index(ComServeVM vm)
        {
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                vm.Volunteers = GetVolunteers(vm.SelectedVolunteerID);

                vm.volunteer = db.Volunteers.Where(x => x.Vol_ID == vm.SelectedVolunteerID).FirstOrDefault();

                var list = db.Volunteer_Hours.Where(pp => pp.Vol_ID == vm.volunteer.Vol_ID && pp.Vol_Start_Time >= vm.DateFrom && pp.Vol_End_Time <= vm.DateTo).ToList().Select(rr => new ReportRecordComServe
                {
                    //Vol_Start_Time = rr.Vol_Start_Time.ToString("dd-MMM-yyyy"),
                    // Vol_End_Time = rr.Vol_Start_Time.ToString("dd-MMM-yyyy"),
                    //Vol_workDate = rr.Date.ToString("dd-MMM-yyyy"),
                    Vol_ID = rr.Vol_ID
                });

                //vm.results["list"] = list.ToList();

                return View(vm);
            }

        }
        private ComServeModel GetDataSet()
        {
            ComServeModel data = new ComServeModel();

            data.Volunteer.Clear();
            data.Volunteer.Rows.Clear();

            //foreach (var item in (IEnumerable<ReportRecord>)TempData["records"])
            //{
            //    DataRow row = data.Volunteer.NewRow();
            //    row["OrderDate"] = item.OrderDate;
            //    row["Amount"] = item.Amount;
            //    row["ShipMethod"] = item.ShipMethod;
            //    row["Employee"] = item.Employee;
            //    row["VendorID"] = item.Vol_ID;
            //    data.Volunteer.Rows.Add(row);
            //}

        }
    }
}
