using AdoptifySystem.Models;
using AdoptifySystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace AdoptifySystem.Controllers.Cassie
{
    public class ReportsController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EmployeeTimesheet1()
        {
            return View();
        }

        [WebMethod]
        [HttpPost]
        public JsonResult EmployeeTimesheet2(DateTime Date)
        {

            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {

                List<TimesheetVM> List = db.TimeSheets.Where(x => x.Check_in == Date).Select(x => new TimesheetVM { Check_in = x.Check_in, Check_out = x.Check_out, Emp_Name = x.Employee.Emp_Name, Emp_Surname = x.Employee.Emp_Surname }).ToList();
                List<string> EmpName = List.Select(x => x.Emp_Name).ToList();
                List<string> EmpSurname = List.Select(x => x.Emp_Surname).ToList();
                List<DateTime> CheckIn = List.Select(x => x.Check_in).ToList();
                List<DateTime> CheckOut = List.Select(x => x.Check_out).ToList();

                return Json(new { empName = EmpName, empSurname = EmpSurname, checkIn = CheckIn, checkOut = CheckOut }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VetAppointList()
        {
            return View();
        }

        [WebMethod]
        [HttpPost]
        public JsonResult VetAppointList1(DateTime Date)
        {

            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                List<VetAppVM> List = db.Vet_Appointment_Master.Where(x => x.AppointmentDate == Date).Select(x => new VetAppVM { Animal_Name = x.Animal.Animal_Name, AppointmentDate = x.AppointmentDate, Kennel_Name = db.Animals.Where(d => d.Animal_ID == x.Animal_ID).Select(d => d.Kennel.Kennel_Name).FirstOrDefault(), Vet_Name = x.Veterinarian.Vet_Name, Vet_Tel = x.Veterinarian.Vet_Tel }).ToList();
                List<DateTime> DateT = List.Select(x => x.AppointmentDate).ToList();
                List<string> AName = List.Select(x => x.Animal_Name).ToList();
                List<string> VName = List.Select(x => x.Vet_Name).ToList();
                List<string> KName = List.Select(x => x.Kennel_Name).ToList();
                List<string> VTel = List.Select(x => x.Vet_Tel).ToList();

                return Json(new { AnName = AName, VetName = VName, kenName = KName, VetTel = VTel, Dates = DateT }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ComServe()
        {
            return View();
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ComServe1(DateTime Date)
        {
            //transactional
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                List<ComServeVM> List = db.Volunteer_Hours.Where(x => x.Vol_workDate == Date).Select(x => new ComServeVM { Vol_Start_Time = x.Vol_Start_Time, Vol_End_Time = x.Vol_End_Time, Vol_Name = x.Volunteer.Vol_Name, Vol_Surname = x.Volunteer.Vol_Surname }).ToList();

                List<DateTime> DateEnd = List.Select(x => x.Vol_End_Time).ToList();
                List<DateTime> DateStart = List.Select(x => x.Vol_Start_Time).ToList();
                List<string> VolName = List.Select(x => x.Vol_Name).ToList();
                List<string> VolSurname = List.Select(x => x.Vol_Surname).ToList();

                return Json(new { dStart = DateStart, dEnd = DateEnd, vName = VolName, vSurname = VolSurname, }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AuditTrail()
        {
            return View();
        }

        [WebMethod]
        [HttpPost]
        public JsonResult AuditTrail1(DateTime Date)
        {
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                List<AuditTrailVM> List = db.Audit_Log.Where(x => x.Auditlog_DateTime == Date).Select(x => new AuditTrailVM { Auditlog_ID = x.Auditlog_ID, Transaction_Type = x.Transaction_Type, Critical_Date = x.Critical_Date, Username = x.User_.Username }).ToList();
                List<int> AuditID = List.Select(x => x.Auditlog_ID).ToList();
                List<string> TransactionType = List.Select(x => x.Transaction_Type).ToList();
                List<string> criticalDate = List.Select(x => x.Critical_Date).ToList();
                List<string> username = List.Select(x => x.Username).ToList();

                return Json(new { aID = AuditID, tType = TransactionType, cDate = criticalDate, uName = username }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EventSale()
        {
            List<Customer_Event> cEvent = db.Customer_Event.ToList();
            List<Event_> Events = db.Event_.ToList();
            EventSaleVM eventModel = new EventSaleVM();

            var tickets = from r in Events
                          join s in cEvent on r.Event_ID equals s.Event_ID
                          select new EventCus { Event = r, CustomerEvent = s };
            return View(new EventSaleVM
            {
                EventCus = tickets

            });
        }


        public ActionResult AnimalDetails()
        {
            Wollies_ShelterEntities db = new Wollies_ShelterEntities();

            List<AnimalVM> List = db.Animals.Select(x => new AnimalVM { Animal_Entry_Date = x.Animal_Entry_Date, Animal_Name = x.Animal_Name, Animal_Type_Name = x.Animal_Type.Animal_Type_Name, Animal_Gender = x.Animal_Gender, Animal_Age = x.Animal_Age, Animal_Sterilization = x.Animal_Sterilization, Animal_Castration = x.Animal_Castration, Animal_Status_Name = x.Animal_Status.Animal_Status_Name, Kennel_Name = x.Kennel.Kennel_Name }).Where((x => x.Animal_Castration == false || x.Animal_Sterilization == false)).ToList();
            ViewBag.Sheets = List;
            return View();
        }


    }
}

