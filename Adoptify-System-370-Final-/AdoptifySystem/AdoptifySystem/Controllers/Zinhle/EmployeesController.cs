using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using Flexible = AdoptifySystem.Models.nickeymodel.Innovation;
using System.IO;
using Google.Authenticator;
using System.Net;
using AdoptifySystem;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace AdoptifySystem.Controllers.Zinhle
{

    public class EmployeesController : Controller
    {
        // GET: Donations
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        static Innovation innovation = new Innovation();
        private const string key = "qaz123!@@)(*";

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
                innovation.Roles = db.Role_.ToList();


                return View(innovation);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("Index");

            }

        }
        [HttpPost]
        public ContentResult AddEmployeein(Employee emp, User_ user, int?[] Role, string Gender, HttpPostedFileBase Contract, FormCollection form)
        {
            try
            {
                db.Database.CommandTimeout = 150;
                Employee saveEmp = new Employee();
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                saveEmp = emp;
                //this is where we convert the contract to add to the database
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(Contract.InputStream))
                {

                    bytes = br.ReadBytes(Contract.ContentLength);
                }
                saveEmp.Emp_Contract_Name = Path.GetFileName(Contract.FileName);
                saveEmp.Emp_Contract_Type = Contract.ContentType;
                saveEmp.Emp_Contract = bytes;

                db.Employees.Add(saveEmp);
                db.SaveChanges();
                //Now we have to store the user
                //first look for the employee that we just added
                Employee old = db.Employees.Where(z => z.Title_ID == saveEmp.Title_ID && z.Emp_Type_ID == saveEmp.Emp_Type_ID && z.Emp_Name == saveEmp.Emp_Name && z.Emp_Email == saveEmp.Emp_Email && z.Emp_Surname == saveEmp.Emp_Surname && z.Emp_IDNumber == saveEmp.Emp_IDNumber).FirstOrDefault();
                //then we add the employee id to the user that we created at the top
                
                if (user == null || Role == null)
                {
                    TempData["SuccessMessage"] = "Succesfully added the employee";
                    return Content("Succesfully added the employee");
                }
                user.Emp_ID = saveEmp.Emp_ID;
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                string UserUniqueKey = (user.Username + key);
                Session["UserUniqueKey"] = UserUniqueKey;
                var setupInfo = tfa.GenerateSetupCode("Wollies Shelter", user.Username, UserUniqueKey, 300, 300);
                saveEmp.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                ViewBag.Qr = setupInfo.QrCodeSetupImageUrl;
                db.Entry(old).CurrentValues.SetValues(saveEmp);
                db.SaveChanges();
                //var md5 = new MD5CryptoServiceProvider();
                //var pass = md5.ComputeHash(Convert.FromBase64String(user.Password));
                //user.Password = pass;
                //we store the info

                db.User_.Add(user);
                db.SaveChanges();
                //we store the User acces that is needed

                foreach (var item in Role)
                {
                    UserRole userRole = new UserRole();
                    userRole.UserID = user.UserID;
                    userRole.Role_ID = item;
                    db.UserRoles.Add(userRole);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Succesfully added the User";
                }
                

                TempData["SuccessMessage"] = "Succesfully added the User";
                //return View("BarCodeGenerated", user);
                return Content("Succesfully added the User");

            }


            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                //return RedirectToAction("AddEmployee");
                return Content("");
            }
           // return new JsonResult { Data = new { status = status } };
        }

        //    public ActionResult AddEmployee(int? Title, int? EmployeeType, Employee emp, User_ user, int?[] Role, string Gender, HttpPostedFileBase Contract)
        //{
        //    Employee saveEmp = new Employee();

        //    try
        //    {
        //        if (Title == null || EmployeeType == null || emp == null)

        //        {
        //            TempData["EditMessage"] = "Please Complete all the relevant information";
        //            return View("AddEmployee", innovation);
        //        }

        //        saveEmp = emp;
        //        //saveEmp.Emp_Gender = Gender;
        //        saveEmp.Title_ID = Title;
        //        saveEmp.Emp_Type_ID = EmployeeType;

        //        //this is where we convert the contract to add to the database
        //        byte[] bytes;
        //        using (BinaryReader br = new BinaryReader(Contract.InputStream))
        //        {

        //            bytes = br.ReadBytes(Contract.ContentLength);
        //        }
        //        saveEmp.Emp_Contract_Name = Path.GetFileName(Contract.FileName);
        //        saveEmp.Emp_Contract_Type = Contract.ContentType;
        //        saveEmp.Emp_Contract = bytes;

        //        db.Employees.Add(saveEmp);
        //        db.SaveChanges();
        //        //Now we have to store the user
        //        //first look for the employee that we just added
        //        Employee searchemp = db.Employees.Where(z => z.Title_ID == saveEmp.Title_ID && z.Emp_Type_ID == saveEmp.Emp_Type_ID && z.Emp_Name == saveEmp.Emp_Name && z.Emp_Email == saveEmp.Emp_Email && z.Emp_Surname == saveEmp.Emp_Surname && z.Emp_IDNumber == saveEmp.Emp_IDNumber).FirstOrDefault();
        //        Employee old = db.Employees.Where(z => z.Title_ID == saveEmp.Title_ID && z.Emp_Type_ID == saveEmp.Emp_Type_ID && z.Emp_Name == saveEmp.Emp_Name && z.Emp_Email == saveEmp.Emp_Email && z.Emp_Surname == saveEmp.Emp_Surname && z.Emp_IDNumber == saveEmp.Emp_IDNumber).FirstOrDefault();
        //        //then we add the employee id to the user that we created at the top
        //        if (searchemp == null)
        //        {
        //            TempData["SuccessMessage"] = "Successfully added the employee";
        //            return View("AddEmployee", innovation);
        //        }
        //        if (user == null || Role == null)
        //        {
        //            TempData["SuccessMessage"] = "Succesfully added the employee";
        //            return View("AddEmployee", innovation);
        //        }
        //        user.Emp_ID = searchemp.Emp_ID;
        //        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                
        //        string UserUniqueKey = (user.Username + key);
        //        Session["UserUniqueKey"] = UserUniqueKey;
        //        var setupInfo = tfa.GenerateSetupCode("Wollies Shelter", user.Username, UserUniqueKey, 300, 300);
        //        searchemp.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
        //        ViewBag.Qr= setupInfo.QrCodeSetupImageUrl;
        //        db.Entry(old).CurrentValues.SetValues(searchemp);
        //        db.SaveChanges();
        //        //var md5 = new MD5CryptoServiceProvider();
        //        //var pass = md5.ComputeHash(Convert.FromBase64String(user.Password));
        //        //user.Password = pass;
        //        //we store the info

        //        db.User_.Add(user);
        //        db.SaveChanges();
        //        //we store the User acces that is needed
        //        User_ searchuser = db.User_.Where(z => z.Emp_ID == searchemp.Emp_ID).FirstOrDefault();

        //        if (searchuser == null)
        //        {

        //            return View("AddEmployee", innovation);
        //        }

        //        foreach (var item in Role)
        //        {
        //            UserRole userRole = new UserRole();
        //            userRole.UserID = searchuser.UserID;
        //            userRole.Role_ID = item;
        //            db.UserRoles.Add(userRole);
        //            db.SaveChanges();
        //            TempData["SuccessMessage"] = "Succesfully added the User";
        //        }
        //        user = searchuser;
                
        //        TempData["SuccessMessage"] = "Succesfully added the User";
        //        return View("BarCodeGenerated", user);

        //    }


        //    catch (Exception e)
        //    {
        //        TempData["EditMessage"] = e.Message;
        //        return RedirectToAction("AddEmployee");
        //    }

        //}

        public ActionResult MaintainEmployees(int? id)
        {
            try
            {
                innovation.Titles = db.Titles.ToList();
                innovation.empTypes = db.Employee_Type.ToList();
                innovation.Roles = db.Role_.ToList();
                db.Database.CommandTimeout = 150;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Employee employee = db.Employees.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                innovation.employee = employee;


                return View(innovation);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("Index");

            }
        }
        [HttpPost]
        public ActionResult MaintainEmployees(Employee emp, User_ user, int?[] Role, string Gender, HttpPostedFileBase Contract)
        {
            
                try
                {
                db.Database.CommandTimeout = 150;
                    Employee searchemployee_type = db.Employees.Find(emp.Emp_ID);
                    if (searchemployee_type == null)
                    {
                        return HttpNotFound();
                    }

                searchemployee_type.Emp_Name = emp.Emp_Name;
                searchemployee_type.Emp_Surname = emp.Emp_Surname;
                searchemployee_type.Emp_Email = emp.Emp_Email;
                searchemployee_type.Emp_IDNumber = emp.Emp_IDNumber;
                searchemployee_type.Emp_ContactNumber = emp.Emp_ContactNumber;
                searchemployee_type.Emp_Type_ID = emp.Emp_Type_ID;
                searchemployee_type.Title_ID = emp.Title_ID;
                //this is where we convert the contract to add to the database
                byte[] bytes;
                if (Contract !=null) {
                    using (BinaryReader br = new BinaryReader(Contract.InputStream))
                    {

                        bytes = br.ReadBytes(Contract.ContentLength);
                    }
                    searchemployee_type.Emp_Contract_Name = Path.GetFileName(Contract.FileName);
                    searchemployee_type.Emp_Contract_Type = Contract.ContentType;
                    searchemployee_type.Emp_Contract = bytes;
                }
                        db.SaveChanges();                   
                //now i have to update user 
                    if (searchemployee_type.User_.Count == 0)
                    {
                        TempData["EditMessage"] = "Employee Succesfully Updated";
                        return View("SearchEmployee", innovation);
                    };
                    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                    string UserUniqueKey = (user.Username + key);
                    Session["UserUniqueKey"] = UserUniqueKey;
                    var setupInfo = tfa.GenerateSetupCode("Wollies Shelter", user.Username, UserUniqueKey, 300, 300);
                    searchemployee_type.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;

                    db.SaveChanges();

                    if (searchemployee_type.User_.Count == 0)
                    {
                        TempData["EditMessage"] = "Employee Succesfully Updated";
                        return View("AddEmployee", innovation);
                    }

                    foreach (var item in Role)
                    {
                       
                        UserRole userRole = new UserRole();
                        userRole.UserID = user.UserID;
                        userRole.Role_ID = item;
                        db.UserRoles.Add(userRole);
                        db.SaveChanges();
                        TempData["EditMessage"] = "Employee Succesfully Updated";
                    }
                   
                    TempData["EditMessage"] = "Employee Succesfully Updated";
                    return View("BarCodeGenerated", user);
                    



                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    
                    return RedirectToAction("Index", "Home");
                }

            return RedirectToAction("Index", "Home");
          
        }
        //BarCodeGenerated
        public ActionResult AddEmployeeType()
        {
            
            return View();
        }
        [HttpPost]
        public ContentResult AddEmployeeType(Employee_Type employee_type)
        {
            try
            {
                List<Employee_Type> Employeetype = new List<Employee_Type>();
                Employeetype = db.Employee_Type.ToList();
                if (Employeetype.Count != 0)
                {
                    int count = 0;
                    foreach (var item in Employeetype)
                    {
                        if (item.Emp_Type_Name == employee_type.Emp_Type_Name)
                        {
                            count++;
                            TempData["EditMessage"] = "There is a duplicate Employee Type Already";
                            return Content("");
                        }
                    }
                    if (count == 0)
                    {
                        db.Employee_Type.Add(employee_type);
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.Employee_Type.Add(employee_type);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = "There was an Error with network please try again: " + e.Message;
                return Content("");
            }
            return Content("");
    }

        public ActionResult SearchEmployee()
       {
            List<Employee> employees = new List<Employee>();
            try
            {
                db.Database.CommandTimeout = 300;
                employees = db.Employees.ToList();
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = e.Message;
                return RedirectToAction("Index", "Home");
            }


            return View(employees);
        }

         public ActionResult DeleteEmployee(int? id)
        {
            if (id != null)
            {
                Employee emp = db.Employees.Find(id);
                int timesheet = emp.TimeSheets.Count();
                int kennel = emp.Emp_Kennel.Count();
                int homechecks = emp.HomeChecks.Count();
                int audit = 0;
                if (emp.User_.Count != 0)
                {
                    User_ user = emp.User_.FirstOrDefault();
                    audit = user.Audit_Log.Count();
                   
                    
                    
                }
                if (timesheet != 0 || kennel != 0|| homechecks != 0 || audit !=0)
                {
                    //you cant delete becasue its referenced to another table
                    ViewBag.err = "You can not delete this";
                    return RedirectToAction("SearchEmployee");
                }
                else
                {
                    db.Database.CommandTimeout = 150;

                    if (emp.User_.Count != 0)
                    {
                        User_ user = emp.User_.FirstOrDefault();
                        foreach (var item in user.UserRoles)
                        {
                            db.UserRoles.Remove(item);
                            if (user.UserRoles.Count == 0)
                            {
                                break;
                            }
                        }
                        db.User_.Remove(user);
                        db.SaveChanges();
                    }
                    
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    return RedirectToAction("SearchEmployee");
                }
            }
            return RedirectToAction("SearchEmployee");
        }
        public ActionResult MaintainEmployeeType(int? id)
        {
            if (id == null)
            {
                TempData["EditMessage"] = "Error is completed";
                return RedirectToAction("SearchEmployee");
            }
            Employee_Type employeetype = db.Employee_Type.Find(id);
            if (employeetype == null)
            {
                TempData["EditMessage"] = "Employee Succesfully Updated";
                return RedirectToAction("Index","Home");
            }
            TempData["SuccessMessage"] = "Employee Succesfully Updated";
            return View(employeetype);
        }
        [HttpPost]
        public ContentResult MaintainEmployeeType(Employee_Type employee_type)
        {
                try
                {
                    Employee_Type searchemployee_type = db.Employee_Type.Find(employee_type.Emp_Type_ID);
                    if (searchemployee_type == null)
                    {
                        TempData["EditMessage"] = "Error is completed";
                    return Content("");
                    }
                    else
                    {
                    searchemployee_type.Emp_Type_Name = employee_type.Emp_Type_Name;
                    searchemployee_type.Emp_Type_Description = employee_type.Emp_Type_Description;
                    db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    TempData["EditMessage"] = e.Message;
                return Content("");
            }
            return Content("");

        }

        public ActionResult SearchEmployeeType()
        {

            ViewBag.errormessage = "";
            List<Employee_Type> employee_Types = new List<Employee_Type>();
            try
            {
                employee_Types = db.Employee_Type.ToList();
                if (employee_Types.Count == 0)
                {

                }
                return View(employee_Types);
            }
            catch (Exception e)
            {
                TempData["EditMessage"] = "there was a network error: " + e.Message;
                return RedirectToAction("Index","Home") ;
            }

        }

        public ActionResult DeleteEmployeeType(int? id)
        {

            if (id != null)
            {
                Employee_Type employee_type = db.Employee_Type.Find(id);
                int count = employee_type.Employees.Count();
                if (count != 0)
                {
                    //you cant delete becasue its referenced to another table
                    ViewBag.err = "You can not delete this";
                    return RedirectToAction("SearchEmployeeType");
                }
                else
                {
                    db.Employee_Type.Remove(employee_type);
                    db.SaveChanges();
                    return RedirectToAction("SearchEmployeeType");
                }
            }
            return RedirectToAction("SearchEmployeeType");
        }
        public ActionResult BarCodeGenerated()
        {
            return View();
        }
    }
}

        


    