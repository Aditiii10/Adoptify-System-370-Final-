using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using AdoptifySystem.ViewModel;
using AdoptifySystem.Models;
using AdoptifySystem;

namespace _370FINAL.Controllers
{
    public class ReportsController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        ///////////////////ActionView///////////////////////
        public ActionResult EmployeeTimesheet()
        {
            return View();
        }
        /////////////////////Json//////////////////////////

        [WebMethod]
        [HttpPost]
        public JsonResult EmployeeTimesheet1(DateTime Date)
        {

            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {

                List<TimesheetVM> List = db.TimeSheets.Where(x => x.Check_in == Date).Select(x => new TimesheetVM { Check_in = x.Check_in,Check_out = x.Check_out, Emp_Name = x.Employee.Emp_Name, Emp_Surname = x.Employee.Emp_Surname }).ToList();
                List<string> EmpName = List.Select(x => x.Emp_Name).ToList();
                List<string> EmpSurname = List.Select(x => x.Emp_Surname).ToList();
                List<string> CheckIn = List.Select(x => x.Check_in.ToString()).ToList();
                List<string> CheckOut = List.Select(x => x.Check_out.ToString()).ToList();
                return Json(new { empName = EmpName, empSurname = EmpSurname, checkIn = CheckIn, checkOut = CheckOut }, JsonRequestBehavior.AllowGet);
            }

        }

        ///////////////////ActionView///////////////////////
         public ActionResult VetAppointList()
         {
            return View();
         }

        /////////////////////Json//////////////////////////
        [WebMethod]
        [HttpPost]
        public JsonResult VetAppointList1()
        {

            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                {
                    DateTime to = DateTime.Today;
                    var appointments = (from vet in db.Vet_Appointment_Master
                                        join ani in db.Animals on vet.Animal_ID equals ani.Animal_ID
                                        join ken in db.Kennels on ani.Kennel_ID equals ken.Kennel_ID
                                        join vett in db.Veterinarians on vet.Vet_ID equals vett.Vet_ID
                                        where (vet.AppointmentDate >= to)
                                        select new
                                        {
                                            AnName = ani.Animal_Name,
                                            VetName = vett.Vet_Name,
                                            kenName = ken.Kennel_Name,
                                            VetTel = vett.Vet_Tel,
                                        }).ToList();

                    return new JsonResult { Data = appointments , JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }

        }

        public ActionResult ComServe()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            Wollies_ShelterEntities db = new Wollies_ShelterEntities();
            var procedures = (from prod in db.Volunteers
                              where prod.Vol_Name.StartsWith(prefix)
                              select new
                              {
                                  label = prod.Vol_Name,
                                  label1 = prod.Vol_Surname,
                                  label2 = prod.Vol_ContactNumber,
                                  label3 = prod.Vol_Email,
                                  label4 = prod.Vol_ID
                              }).ToList();

            return Json(procedures);
        }

        [WebMethod]
        [HttpPost]
        public JsonResult ComServe1(int ID)
        {
            
            //transactional
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                List<ComServeVM> List = db.Volunteer_Hours.Where(x => x.Vol_ID == ID).Select(x => new ComServeVM {Vol_workDate = x.Vol_workDate, Vol_Start_Time = x.Vol_Start_Time, Vol_End_Time = x.Vol_End_Time , Count = (x.Vol_End_Time.Hours - x.Vol_Start_Time.Hours).ToString()}).ToList();

                List<string> DateWork = List.Select(x => x.Vol_workDate.ToShortDateString()).ToList();
                List<string> EndTime = List.Select(x => x.Vol_End_Time.ToString()).ToList();
                List<string> StartTime = List.Select(x => x.Vol_Start_Time.ToString()).ToList();




                return Json(new {dWork = DateWork, dStart = StartTime, dEnd = EndTime}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AuditTrail()
        {
            return View();
        }

        public JsonResult AuditTrail1()
        {
            DateTime LastWeek = DateTime.Today.AddDays(-7);
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                List<AuditTrailVM> List = db.Audit_Log.Where(x => x.Auditlog_DateTime > LastWeek).Select(x => new AuditTrailVM { Auditlog_DateTime = x.Auditlog_DateTime, Transaction_Type = x.Transaction_Type, Critical_Date = x.Critical_Date, Emp_IDNumber = x.User_.Employee.Emp_IDNumber ,Emp_Name = x.User_.Employee.Emp_Name, Emp_Surname= x.User_.Employee.Emp_Surname }).ToList();
                List<int> AuditID = List.Select(x => x.Auditlog_ID).ToList();
                List<string> TransactionType = List.Select(x => x.Transaction_Type).ToList();
                List<string> criticalDate = List.Select(x => x.Critical_Date).ToList();
                List<string> empID = List.Select(x => x.Emp_IDNumber).ToList();
                List<string> empName = List.Select(x => x.Emp_Name).ToList();
                List<string> empSurname = List.Select(x => x.Emp_Surname).ToList();
                

                return Json(new { aID = AuditID, tType = TransactionType, cDate = criticalDate, eID = empID, eName = empName, eSurname = empSurname  }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AnimalDetails()
        {

            return View();
        }

        public JsonResult AnimalDetails1()
        {
            List<AnimalVM> List = db.Animals.Where(x => x.Animal_Castration == false || x.Animal_Sterilization == false && x.Animal_Status_ID != 4 && x.Animal_Status_ID != 5 && x.Animal_Status_ID != 6).Select(x => new AnimalVM { Animal_Entry_Date = x.Animal_Entry_Date, Animal_Name = x.Animal_Name, Animal_Type_Name = x.Animal_Type.Animal_Type_Name, Animal_Age = x.Animal_Age, Animal_Gender = x.Animal_Gender, Animal_Castration = x.Animal_Castration, Animal_Sterilization = x.Animal_Sterilization, Kennel_Name = x.Kennel.Kennel_Name}).ToList();
            List<string> DateT = List.Select(x => x.Animal_Entry_Date.ToShortDateString()).ToList();
            List<string> anName = List.Select(x => x.Animal_Name).ToList();
            List<string> anType = List.Select(x => x.Animal_Type_Name).ToList();
            List<int> anAge = List.Select(x => x.Animal_Age).ToList();
            List<string> anGender = List.Select(x => x.Animal_Gender).ToList();
            List<bool> anCast = List.Select(x => x.Animal_Castration).ToList();
            List<bool> anSter = List.Select(x => x.Animal_Sterilization).ToList();
            List<string> anKennel = List.Select(x => x.Kennel_Name).ToList();

            return Json(new { aDate = DateT, aName = anName, aType = anType, aAge = anAge,  aGender = anGender, aCast = anCast, aSter = anSter, aKennel = anKennel }, JsonRequestBehavior.AllowGet);
        }

    }

}