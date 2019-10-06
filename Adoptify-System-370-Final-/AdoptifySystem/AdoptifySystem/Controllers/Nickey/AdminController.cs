using AdoptifySystem.Models.nickeymodel;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using AdoptifySystem;
using System.Web.Helpers;

namespace AdoptifySystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private const string key = "qaz123!@@)(*";//this needs to be generated for each person so that it is unique barcode
        /* any 10-12 char string for use as private key in google authenticator
        use later for generate Google authenticator code.*/
        
        //this is the Db that i will be unstatnitatiung to use thought the whole controller
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Flexible flex = new Flexible();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User_ login)
        {

            //bool status = false;
            Wollies_ShelterEntities db = new Wollies_ShelterEntities();
            List<User_> Users;
            try
            {
                
                Users = db.User_.ToList();
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw;
            }

            foreach (var item in Users)
            {
                var hashed = Crypto.Hash(login.Password, "MD5");
                if (item.Username == login.Username && item.Password == hashed)
                {
                    //we check if he is available
                    if (item.FirstTime == false)
                    {
                        Session["Username"] = login.Username;
                        flex.currentuser = item;
                        Session["ID"] = item.UserID;
                        //2FA Setup
                        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                        string UserUniqueKey = (login.Username + key);
                        Session["UserUniqueKey"] = UserUniqueKey;
                        TempData["LoginSuccessMessage"] = "Logged in Successfully";
                        return RedirectToAction("Index", "Home");
                        
                    }
                    else
                    {
                        Session["Username"] = login.Username;
                        flex.currentuser = item;
                        Session["TempID"] = item.UserID;
                        //2FA Setup
                        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                        string UserUniqueKey = (login.Username + key);
                        Session["UserUniqueKey"] = UserUniqueKey;
                        return View("Authorize");
                    }

                }
                else
                {
                    ViewBag.LoginError = "Incorrect Password or Username!! Please Try Again!";
                }
            }
            return View();
        }

        public ActionResult Authorize()
        {
            if (Convert.ToInt32(Session["TempID"]) == 0)
            {
                return RedirectToAction("Logout");
            }
            return View();
        }
        //authorized user will be redirected to after successful login
        public ActionResult MyProfile()
        {
            if (Session["Username"] == null || Session["IsValid2FA"] == null || !(bool)Session["IsValid2FA"])
            {
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Welcome " + Session["Username"].ToString();
            return View();
        }
        [HttpPost]
        //here this is where we go and authorize the code that was genereated on the user mobile application
        public ActionResult Verify2FA(string token)
        {
            try
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                string UserUniqueKey = Session["UserUniqueKey"].ToString();
                bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
                
                if (isValid)
                {
                    int tempid = Convert.ToInt32(Session["TempID"]);

                    User_ user = db.User_.Find(tempid);
                    if (user == null)
                    {
                        return RedirectToAction("Logout");
                    }
                    user.FirstTime = false;
                    db.SaveChanges();
                    Session["ID"] = user.UserID;
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login", "Admin");
            }
            catch (Exception)
            {

                throw;
            } //var token = Request["passcode"];
            
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("Login");
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            //bool status = false;

            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {

             
                var account = dc.Employees.Where(a => a.Emp_Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    var user = dc.User_.Where(a => a.Emp_ID == account.Emp_ID).FirstOrDefault();
                    if (user != null)
                    {
                        //Send email for reset password
                        string resetCode = Membership.GeneratePassword(12, 1);
                        SendVerificationLinkEmail(account.Emp_Email, resetCode, "ResetPassword");
                        user.Password = resetCode;
                        //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                        //in our model class in part 1
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "Reset password link has been sent to your email.";
                    }
                    else
                    {
                        message = "Account not found";
                    }
                }
                else
                {
                    message = "Account not found";
                }

            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult Checkin()
        {
            try
            {
                if (Convert.ToInt32(Session["ID"]) == 0)
                {
                    return RedirectToAction("Logout");
                }
                if (flex.Authorize(Convert.ToInt32(Session["ID"]), 1))
                {
                    List<Employee> emp = new List<Employee>();
                    emp = db.Employees.Where(z => z.Employee_Status == true).ToList();
                    flex.employeelist = emp;
                    flex.employee = null;
                    ViewBag.Time = DateTime.Now;
                    return View(flex);
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }
        [HttpPost]
        public ActionResult Checkin(int? id)
        {

            try
            {
                id++;
                DateTime datenow = DateTime.Now;
                TimeSheet time = new TimeSheet();
                time.Emp_ID = flex.employee.Emp_ID;
                time.Check_in = datenow;

                db.TimeSheets.Add(time);
                db.SaveChanges();
                var emp = db.Employees.Where(z => z.Emp_ID == flex.employee.Emp_ID).FirstOrDefault();
                //var empold = db.Employees.Where(z => z.Emp_ID == flex.employee.Emp_ID).FirstOrDefault();
                if (emp == null)
                {
                    ViewBag.err = "Employee is not found";
                    return RedirectToAction("Index", "Home");
                }
                emp.Employee_Status = false;
                //db.Entry(emp).CurrentValues.SetValues(empold);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
            


        }
        [HttpPost]
        public ActionResult GetUserDetails(int? id, string button)
        {
            try
            {
                //int userId = Convert.ToInt32(Request.Form["id"]);
                //fetch the data by userId and assign in a variable, for ex: myUser
                //Flexible myUser = new Flexible();
                if (id == null)
                {
                    return RedirectToAction("Checkin");
                }
                var emp = flex.employeelist.Where(z => z.Emp_ID == id).FirstOrDefault();
                if (id == null)
                {
                    ViewBag.err = "Employee not found";
                    return RedirectToAction("Checkin");
                }
                flex.employee = emp;
                if (button == "checkout")
                {
                    return View("Checkout", flex);
                }
                if (button == "checkin")
                {
                    return View("Checkin", flex);
                }
                return View("Search");
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult Checkout()
        {
            try
            {
                if (Convert.ToInt32(Session["ID"]) == 0)
                {
                    return RedirectToAction("Logout");
                }
                if (flex.Authorize(Convert.ToInt32(Session["ID"]), 1))
                {
                    List<Employee> emp = new List<Employee>();
                    emp = db.Employees.Where(z => z.Employee_Status == false).ToList();
                    flex.employeelist = emp;
                    flex.employee = null;
                    ViewBag.Time = DateTime.Now;
                    return View(flex);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Checkout(int? id)
        {
            try
            {
               
                id++;
                DateTime datenow = DateTime.Now;
                TimeSheet time = new TimeSheet();
                time.Emp_ID = flex.employee.Emp_ID;
                time.Check_out = datenow;
                //find the timesheet
                var timesheets = db.TimeSheets.Where(z => z.Emp_ID == time.Emp_ID && z.Check_in != null).ToList();
                //var oldtimesheet = db.TimeSheets.Where(z => z.Emp_ID == time.Emp_ID && time.Check_in.Equals(datenow.Date)).FirstOrDefault();
                if (timesheets == null)
                {
                    ViewBag.err = "Timesheet is not found";
                    return View(flex);
                }
                
                foreach (var item in timesheets)
                {
                    DateTime checkin = Convert.ToDateTime(item.Check_in);
                    DateTime checkout = Convert.ToDateTime(time.Check_out);
                    if (checkin.Date == checkout.Date)
                    {
                        var timesheet = db.TimeSheets.Find(item.TimeSheet_ID);
                        
                        timesheet.Check_out = time.Check_out;
                        //db.Entry(oldtimesheet).CurrentValues.SetValues(timesheet);
                        db.SaveChanges();
                       
                        break;
                    }
                    
                }
               
                var emp = db.Employees.Where(z => z.Emp_ID == flex.employee.Emp_ID).FirstOrDefault();
                if (emp == null)
                {
                    ViewBag.err = "Employee is not found";
                    return View();
                }
                //emp.Employee_Status_ID = false;
                emp.Employee_Status = true;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        [HttpPost]
        public ActionResult ChangePassword(int id)
        {
            try
            {
                if (id != 0)
                {
                    int test = Convert.ToInt32(Session["ID"].ToString());
                    User_ user = db.User_.Find(test);
                    if (user == null)
                    {
                        return RedirectToAction("Logout");
                    }
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Logout");
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public ActionResult AddUserRole()
        {
            try
            {
                flex.subsystemslist = db.Subsystems.ToList();
                if (flex.subsystemslist == null)
                {
                    return RedirectToAction("Search");
                }
                return View(flex);
            }
            catch (Exception)
            {

                throw;
            }
            
           
        }
        [HttpPost]
        public ActionResult AddUserRole(Role_ role,int[] Subsystem_Id, string button)
        {
            ViewBag.errorMessage = "";
            //Donation_Type asd = new Donation_Type();
              try
                {
                if (button == "Save")
                {

                    List<Role_> Roles = new List<Role_>();
                    Roles = db.Role_.ToList();
                    if (Roles.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in Roles)
                        {
                            if (item.Role_Name == role.Role_Name)
                            {
                                count++;
                                ViewBag.errorMessage = "There is a duplicate User Role Already";
                                return View(flex);
                            }

                        }
                        if (count == 0)
                        {
                            db.Role_.Add(role);
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        db.Role_.Add(role);
                        db.SaveChanges();


                    }
                    if (Subsystem_Id.Length > 0)
                    {
                        foreach (var id in Subsystem_Id)
                        {
                            SubsystemRole test = new SubsystemRole();
                            test.Role_ID = role.Role_ID;
                            test.Subsystem_Id = id;
                            db.SubsystemRoles.Add(test);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("SearchUserRole");
                }
                else if (button == "Cancel")
                {

                    return RedirectToAction("SearchUserRole");
                }
            }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "There was an Error with network please try again: " + e.Message;
                    return View();
                }
                return RedirectToAction("SearchUserRole");
           
            
        }

        public ActionResult SearchUserRole()
        {

            ViewBag.errormessage = "";
            List<Role_> roles = new List<Role_>();
            try
            {
                roles = db.Role_.ToList();
                if (roles.Count == 0)
                {

                }
                return View(roles);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
            }

        }
        [HttpPost]
        public ActionResult SearchUserRole(string search)
        {

            ViewBag.errormessage = "";
            List<Role_> roles = new List<Role_>();
            try
            {
                roles = db.Role_.ToList();
                if (roles.Count == 0)
                {

                }
                return View(roles);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                throw;
            }

        }


        public ActionResult MaintainUserRole(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("SearchUserRole");
                }
                Role_ role = db.Role_.Find(id);
                 var subsystems = db.Subsystems.ToList();
                if (role == null || subsystems == null)
                {
                    return RedirectToAction("SearchUserRole");
                }
                flex.role = role;
                flex.subsystemslist = subsystems;
                return View(flex);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        [HttpPost]
        public ActionResult MaintainUserRole(Role_ role, int[] Subsystem_Id, string button)
        {
            ViewBag.errorMessage = "";
            //Donation_Type asd = new Donation_Type();
            try
            {
                if (button == "Save")
                {

                    List<Role_> Roles = new List<Role_>();
                    Roles = db.Role_.ToList();
                    Role_ dbrole = db.Role_.Find(role.Role_ID);
                    bool isvalid = true;
                    if (Roles.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in Roles)
                        {
                            if (item.Role_Name == role.Role_Name && item.Role_ID != role.Role_ID)
                            {
                                count++;
                                ViewBag.errorMessage = "There is a duplicate User Role Already";
                                return View(flex);
                            }

                        }
                        if (count == 0)
                        {


                            dbrole.Role_Name = role.Role_Name;

                            db.SaveChanges();
                            
                        }
                    }
                    
                    if (dbrole.SubsystemRoles.Count > 0 || Subsystem_Id.Length > 0)
                    {
                        //means we have to delete the user roles
                        if (Subsystem_Id.Length == 0)
                        {

                            return View(flex);
                                
                           
                        }
                        else
                        {
                            foreach (var newid in Subsystem_Id)
                            {
                                int count = 0;
                                //this compare current to see if there are any similarity itherwise we add them
                                foreach (var current_role in dbrole.SubsystemRoles)
                                {
                                    if (current_role.Subsystem_Id == newid)
                                    {
                                        //this means there is a duplicate
                                        count++;
                                        break;
                                    }
                                }
                                if (count == 0)
                                {
                                    SubsystemRole test = new SubsystemRole();
                                    test.Role_ID = role.Role_ID;
                                    test.Subsystem_Id = newid;
                                    db.SubsystemRoles.Add(test);
                                    db.SaveChanges();
                                  
                                }
                            }
                            List<SubsystemRole> delete = new List<SubsystemRole>();
                            //now here we gonna check if he removed
                            foreach (var current_role in dbrole.SubsystemRoles)
                            {
                                int count = 0;
                                //this compare current to see if there are any similarity itherwise we remove them
                                foreach (var newid in Subsystem_Id)
                                {
                                    if (current_role.Subsystem_Id == newid)
                                    {
                                        //this means there is a duplicate
                                        count++;
                                        break;
                                    }
                                }
                                if (count == 0)
                                {
                                    delete.Add(current_role);
                                   

                                }
                            }
                            foreach (var deletetime in delete)
                            {
                                deletetime.Role_ = null;
                                deletetime.Subsystem = null;
                                db.SubsystemRoles.Remove(deletetime);
                                db.SaveChanges();
                            }
                        }
                            
                     
                        //foreach (var id in Subsystem_Id)
                        //{
                        //    SubsystemRole test = new SubsystemRole();
                        //    test.Role_ID = role.Role_ID;
                        //    test.Subsystem_Id = id;
                        //    db.SubsystemRoles.Add(test);
                        //    db.SaveChanges();
                        //}
                    }
                    return RedirectToAction("SearchUserRole");
                }
                else if (button == "Cancel")
                {

                    return RedirectToAction("SearchUserRole");
                }
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = "There was an Error with network please try again: " + e.Message;
                return View(flex);
            }
            return RedirectToAction("SearchUserRole");


        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("u17136319@tuks.co.za");
                mail.To.Add(emailID);
                mail.Subject = "Wollies Animal Shelter Passeword Reset Code";
                mail.Body = "<h1>Hello There!</h1><br><h3>Please Use the New Password Below to Login:<br>" + "  " + activationCode + "</h3>";
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("u17136319@tuks.co.za", "Urahara123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            //};

            //using (var message = new MailMessage(fromEmail, toEmail)
            //{
            //    Subject = subject,
            //    Body = body,
            //    IsBodyHtml = true
            //})
            //    smtp.Send(message);
        }

 
    }
}