using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using Flexible = AdoptifySystem.Models.nickeymodel.Innovation;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class EmployeesController : Controller
    {
        // GET: Donations
        Wollies_ShelterEntities1 db = new Wollies_ShelterEntities1();
        public static Innovation innovation = new Innovation();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddEmployee()
        {

            try
            {
                innovation.Titles = db.Titles.ToList();
                innovation.empTypes = db.Employee_Type.ToList();



                //innovation.empTypes = db.empTypes.ToList();
                return View(innovation);
            }
            catch (Exception e)
            {

                return RedirectToAction("Index");

            }

        }
        [HttpPost]
        public ActionResult AddEmployee(string Title, string EmployeeType, Employee emp, Role_ userRole, string[] Role, string Gender, HttpPostedFileBase Contract)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(Contract.InputStream))
            {
                bytes = br.ReadBytes(Contract.ContentLength);
            }
            emp.Emp_Contract = bytes;
            try
            {
                //TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                //string UserUniqueKey = (emp.Username + key);
                //Session["UserUniqueKey"] = UserUniqueKey;
                //var setupInfo = tfa.GenerateSetupCode("Wollies Shelter", emp.Username, UserUniqueKey, 300, 300);
                //ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                //ViewBag.SetupCode = setupInfo.ManualEntryKey;
                ////message = "Credentials are correct";
                int id = Convert.ToInt32(emp.Emp_IDNumber);
                return RedirectToAction("AddEmployee");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index");

            }

        }

    }
}