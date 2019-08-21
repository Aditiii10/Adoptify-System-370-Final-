using AdoptifySystem.Models;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

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
            var time = DateTime.Now;
            //var nextime = DateTime.Now + 30; 
            //while (time == )
            //{
            
            //}
            //ViewBag.Time = await time;
            return View();
        }
        [HttpPost]
        public ActionResult Login(User_ login)
        {
            
            bool status = false;
            Wollies_ShelterEntities db = new Wollies_ShelterEntities();
            //check username and password form our database here
            //for demo I am going to use Admin as Username and Password1 as Password static value
            List<User_> Users;
            try
            {
                //User_ user = new User_();
                //user.Username = "Divin";
                //user.Password = "food";
                //db.User_.Add(user);
                //db.SaveChanges();
                Users = db.User_.ToList();

            }
            catch (Exception e)
            {

                throw ;
            }
            
            foreach (var item in Users)
            {
                if (item.Username == login.Username && item.Password == login.Password)
                {
                    status = true; // show 2FA form
                    Session["Username"] = login.Username;

                    //2FA Setup
                    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                    string UserUniqueKey = (login.Username + key);
                    Session["UserUniqueKey"] = UserUniqueKey;
                    var setupInfo = tfa.GenerateSetupCode("Wollies Shelter", login.Username, UserUniqueKey, 300, 300);
                    ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                    ViewBag.SetupCode = setupInfo.ManualEntryKey;
                    //message = "Credentials are correct";
                    return View("Authorize",setupInfo);
                }
            }
            return View();
        }

        public ActionResult Authorize()
        {
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
            //var token = Request["passcode"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
            if (isValid)
            {
                Session["IsValid2FA"] = true;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult Logout()
        {
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
            bool status = false;

            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {

                //adding a employee
                //Employee emp = new Employee();
                //emp.Emp_Name = "jem";
                //emp.Emp_Email = "jemimakola99@gmail.com";
                //dc.Employees.Add(emp);
                //dc.SaveChanges();

                //User_ useremp = new User_();
                //useremp.Emp_ID = 1;
                //useremp.Username = "nick";
                //useremp.Password = "john";
                //dc.User_.Add(useremp);
                //dc.SaveChanges();
                var account = dc.Employees.Where(a => a.Emp_Email == EmailID).FirstOrDefault();
                if(account != null)
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
                        message = "Reset password link has been sent to your email id.";
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

            //Employee emp = new Employee();
            //emp.Emp_Name = "nick";
            //emp.Emp_Surname = "Sibanyoni";
            //db.Employees.Add(emp);
            //Employee emp1 = new Employee();
            //emp1.Emp_Name = "james";
            //emp1.Emp_Surname = "High";
            //db.Employees.Add(emp1);
            //Employee emp2 = new Employee();
            //emp2.Emp_Name = "Mercury";
            //emp2.Emp_Surname = "Freddy";
            //db.Employees.Add(emp2);
            //db.SaveChanges();

            flex.employeelist = db.Employees.ToList();
            flex.employee = null;
            return View(flex);
        }
        [HttpPost]
        public ActionResult Checkin(string id)
        {
            if(id != "")
            {
               DateTime datenow = DateTime.Now;
               int employeeid = Convert.ToInt32(id);
                
                //TimeSheet time1 = new TimeSheet();
                //time1.Check_in = Convert.ToDateTime("10/06/2019 8:21:00");
                //time1.Emp_ID = 1;
                //db.TimeSheets.Add(time1);
                //db.SaveChanges();
                //TimeSheet time2 = new TimeSheet();
                //time2.Check_in = Convert.ToDateTime("10/02/2019 8:21:00");
                //time2.Emp_ID = 2;
                //db.TimeSheets.Add(time2);
                //TimeSheet time3 = new TimeSheet();
                //time3.Check_in = Convert.ToDateTime("10/01/2019 8:21:00");
                //time1.Emp_ID = 2;
                //db.TimeSheets.Add(time3);
                //TimeSheet time4 = new TimeSheet();
                //time4.Check_in = Convert.ToDateTime("10/08/2018 8:21:00");
                //time4.Emp_ID = 1;
                //db.TimeSheets.Add(time4);
                //db.SaveChanges();
                //TimeSheet time5 = new TimeSheet();
                //time5.Check_in = Convert.ToDateTime("10/09/2018 8:21:00");
                //time5.Emp_ID = 3;
                //db.TimeSheets.Add(time5);
                //TimeSheet time6 = new TimeSheet();
                //time6.Check_in = Convert.ToDateTime("10/07/2018 8:21:00");
                //time6.Emp_ID = 4;
                //db.TimeSheets.Add(time6);
                //db.SaveChanges();

                //now we search in the timesheet for this person to see if they are already checked in
                List<TimeSheet> time = db.TimeSheets.Where(a=>a.Emp_ID == employeeid).ToList();
                if (time.Count !=0)
                {
                    List<string> Year = new List<string>();
                    //here we look at each item to see if they checked in attribute is same as today date.
                    foreach (var item in time)
                    {
                        DateTime date = (DateTime)item.Check_in;//here we get the date
                        string pastdate = date.ToString("MM/dd/yyyy");
                        string today = datenow.ToString("MM/dd/yyyy");
                        //here we compare the current item date with today's date
                        if (pastdate == today)
                        {
                            Year.Add(pastdate);
                        }

                        
                    }//I use this to see if the list is populated if is then the peron has already checked in today
                    if (Year.Count == 0)
                    {
                        TimeSheet timeSheet = new TimeSheet();
                        timeSheet.Emp_ID = employeeid;
                        timeSheet.Check_in = datenow;
                        db.TimeSheets.Add(timeSheet);
                        return RedirectToAction("Checkout");
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Checkin");
            }
            
        }
        [HttpPost]
        public ActionResult GetUserDetails()
        {
            int userId = Convert.ToInt32(Request.Form["id"]);
            //fetch the data by userId and assign in a variable, for ex: myUser
            //Flexible myUser = new Flexible();
                flex.employee = db.Employees.Find(userId);
            return View("Checkin", flex);
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult Checkout(string id)
        {
            DateTime datenow = DateTime.Now;
            int employeeid = Convert.ToInt32(id);

            List<TimeSheet> time = db.TimeSheets.Where(a => a.Emp_ID == employeeid).ToList();
            if (time.Count != 0)
            {
                List<string> Year = new List<string>();
                //here we look at each item to see if they checked in attribute is same as today date.
                foreach (var item in time)
                {
                    //for each item we need to look for the one that has no checkout value but has a checkin value that has same day
                    DateTime checkin = (DateTime)item.Check_in;//here we get the date
                    string storeddate = checkin.ToString("MM/dd/yyyy");
                    string today = datenow.ToString("MM/dd/yyyy");
                    //here we compare the current item date with today's date
                    if (storeddate == today)
                    {
                        //the date we get here is the one from checkout to see if teh re is a checkout date already
                        DateTime checkout = (DateTime)item.Check_out;
                        string pastdate = checkout.ToString("MM/dd/yyyy");
                        if (pastdate == today)
                        {
                            Year.Add(today);
                        }
                    }


                }//I use this to see if the list is populated if is then the peron has already checked in today
                if (Year.Count == 0)
                {
                    TimeSheet timeSheet = new TimeSheet();
                    timeSheet.Emp_ID = employeeid;
                    timeSheet.Check_in = datenow;
                    db.TimeSheets.Add(timeSheet);
                    db.SaveChanges();
                    return RedirectToAction("Checkout");
                }
                else
                {

                }
            }
            else
            {

            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult AddUserRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUserRole(Role_ role, string button)
        {
            ViewBag.errorMessage = "";
            //Donation_Type asd = new Donation_Type();
            if (button == "Save")
            {
                try
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
                                ViewBag.errorMessage = "There is a duplicate Donation Type Already";
                                return View();
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

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "There was an Error with network please try again: " + e.Message;
                    return View();
                }

            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
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

        public ActionResult MaintainUserRole(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role_ role = db.Role_.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost]
        public ActionResult MaintainUserRole(Role_ role)
        {

            return View();
        }

    [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
           using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("u17136319@tuks.co.za");
                mail.To.Add(emailID);
                mail.Subject = "Hello World";
                mail.Body = "<h1>Hello Jem</h1><br><br><p>please find your code attached</p>";
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