using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using AdoptifySystem.Models;
using AdoptifySystem;
using Twilio;
using Twilio.TwiML;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Nexmo;
using Nexmo.Api;
using Twilio.AspNet.Mvc;

namespace AdoptifySystem.Controllers
{
    [AllowAnonymous]
    public class AdoptionsController : Controller
    {
        
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();
       static  List<Adoption> myList = new List<Adoption>();
       static List<Animal> animal2 = new List<Animal>();
       static List<Animal> animalsss = new List<Animal>();
        static Adoption adoption = new Adoption();
        static int Id = 0;
        // GET: Adoptions
        public ActionResult Index(string searchBy="", string search="")
        {
            //var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).Include(a => a.Payment).Include(a => a.Adoption_Status);
           
            try
            {
                //if (searchBy == "Animal_Name")
                //    return View(db.Adoptions.Where(x => x.Animal.Animal_Name == search || search == null).ToList());
                //else
                //    return View(db.Adoptions.Where(x => x.Adopter.Adopter_Name == search || search == null).ToList());
            return View(db.Adoptions.ToList());

                //return View();

            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }

        }
      

        public ActionResult HomeCheckSchedule(int? id)
        {
        try { 
            List<Adoption> adoption1 = db.Adoptions.ToList();
                Id = Convert.ToInt32(id);
                Adoption adoption = db.Adoptions.Find(id);//Display the animal details object
            
            if (adoption != null)
                {
                    ViewBag.ID = id;
                    ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                    ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old";
                    ViewBag.AnimalImage = adoption.Animal.Animal_Image;
                    return View("HomeCheckSchedule");
                }

                if (id == null)
                {
                    return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {

            }

            return View("HomeCheckSchedule");
        }
        public ActionResult Save(String NDate="")
        {
            try
            {
            DateTime dd = new DateTime();
            Adoption adoption = db.Adoptions.Find(Id);
            
                if (NDate != "")
                {
                    String year = NDate.Substring(0, 4);
                    String month = NDate.Substring(5, 2);
                    String day = NDate.Substring(8, 2);
                    dd = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                }


                if (adoption == null)
                {

                    return HttpNotFound();
                }
                adoption.Adopt_Status_ID = 2;

                HomeCheck obj = new HomeCheck();
                obj.Adoption_ID = Id;
                obj.HomeCheck_Datetime = dd;
                db.HomeChecks.Add(obj);
                db.SaveChanges();
                //Sending SMS
                var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                var authToken = "4186739dfb2554741e7dff014074ff82";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    to: new Twilio.Types.PhoneNumber("+27676367506"),
                    from: new Twilio.Types.PhoneNumber("+14245431153"),
                    body: "CONGRATULATION!" + " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname +" "+ " You have Successfully Scdueled a Homecheck Appointment for the date of " + " " + NDate + " " + "! From Wollies Animal Shelter."
                );
                myList = db.Adoptions.ToList();
                TempData["HCSCMessage"] = "Please Save The Current HomeCheck on the Calendar Schedular";
                TempData["HomeCheckMessage"] = "HomeCheck Successfully Booked";
                //return View("HomeCheckSchedule");
               
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                // return View("Error");
            }
            finally
            {

            }
            //return View("HomeCheckHistory");
            return View("Index", myList);
        }
        public ActionResult HomeCheckReport(int? id)
        {
           try { 
               if (id != 0)
               {
                    Adoption adoption = db.Adoptions.Find(id);
                    ViewBag.ID = adoption.Adoption_ID;
                   ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                  ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" + " " + adoption.Animal.Animal_Image;
              }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult HomeCheckReport(int? id, string val ="")
        {
          
            List<Adoption> adoption1 = db.Adoptions.ToList();
            bool flag1 = false;
            Id = Convert.ToInt32(id);
            Adoption adoption = db.Adoptions.Find(id);
            Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
           try {  
            if (adoption != null)
            {
                
                if (val== "true")
                {
                    val = "true";
                    flag1 = true;
                    
                    adoption.Adopt_Status_ID = 3;
                    adoption.Animal.Animal_Status_ID = 2;
                    TempData["HomeCheckReportMessage"] = "HomeCheck Report Success ";
                    aaa.Adopt_Status_ID = 3;
                    db.SaveChanges();

                        HomeCheckReport obj = new HomeCheckReport();

                        //AdoptionPayment obj = db.AdoptionPayments.SingleOrDefault(x => x.Adoption_ID == id);
                        obj.HomeCheckStatus = flag1;
                        obj.Adoption_ID = Id;
                        db.HomeCheckReports.Add(obj);
                        db.SaveChanges();



                        //Sending SMS
                        var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                    var authToken = "4186739dfb2554741e7dff014074ff82";
                    TwilioClient.Init(accountSid, authToken);
                    var message = MessageResource.Create(
                        to: new Twilio.Types.PhoneNumber("+27676367506"),
                        from: new Twilio.Types.PhoneNumber("+14245431153"),
                            body: "CONGRATULATION!" + " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + "Your HomeCheck Report was Successful And Approved. From Wollies Animal Shelter!"
                    );

                   // return View("FileView", "HCReportFile");
                    return View("Index",db.Adoptions);
                }
                else
                {
                    flag1 = false;
                    val = "false";
                    adoption.Adopt_Status_ID = 4;
                    adoption.Animal.Animal_Status_ID = 2;
                    TempData["HomeCheckReportErrorMessage"] = "HomeCheck Report Failed ";
                    aaa.Adopt_Status_ID = 4;
                    adoption.Adoption_Status.Adopt_Status_ID = 4;
                    db.SaveChanges();

                        HomeCheckReport obj = new HomeCheckReport();
                        obj.HomeCheckStatus = flag1;
                        obj.Adoption_ID = Id;
                        db.HomeCheckReports.Add(obj);
                        db.SaveChanges();


                        //Sending SMS
                        var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                    var authToken = "4186739dfb2554741e7dff014074ff82";
                    TwilioClient.Init(accountSid, authToken);
                    var message = MessageResource.Create(
                        to: new Twilio.Types.PhoneNumber("+27676367506"),
                        from: new Twilio.Types.PhoneNumber("+14245431153"),
                            body: "We are sorry to inform you"+ " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " +adoption.Adopter.Adopter_Surname+" "+ "that Your HomeCheck Report was unsuccessful And Dispproved. From Wollies Animal Shelter!"
                    );
                }


                ViewBag.ID = adoption.Adoption_ID;
                ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" + " " + adoption.Animal.Animal_Image;

                
                //return View("FileView", "HCReportFile");

                return View("Index",db.Adoptions);
            }
           
            if (id == null)
            {
                return View("Index"); //new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData["HomeCheckReportMessage"] = "HomeCheck Successfully Reported";
            }
           catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
               // return View("Error");
            }
    
            return View("Index");
        }



        public ActionResult CaptureAdoptionPayment(int? id)
        {
            try { 
            adoption = db.Adoptions.Find(id);//Display the animal details object
            
            if (adoption != null)
            {
                ViewBag.ID = id;
                ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old";
                ViewBag.Price = adoption.Animal.Animal_Type.Price;

            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View("CaptureAdoptionPayment");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaptureAdoptionPayment(int? id, string Payment="")
        {
            try { 
                 adoption = db.Adoptions.Find(id);//Display the animal details object

            
                if (adoption != null)
                {
                ViewBag.ID = id;
                ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" + " " + adoption.Animal.Animal_Image;
                ViewBag.Price = adoption.Animal.Animal_Type.Price;
           
                if (Payment=="Cash")
                {
                    Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
                        if (aaa != null)
                        {
                            aaa.Adopt_Status_ID = 5;
                            //AdoptionPayment obj = db.AdoptionPayments.SingleOrDefault(x => x.Adoption_ID == id);
                            AdoptionPayment obj = new AdoptionPayment();
                            obj.Adoption_Fee = aaa.Animal.Animal_Type.Price;
                            obj.Adoption_ID = id;
                            obj.Animal_Type_ID = aaa.Animal.Animal_Type_ID;
                            obj.Payment_Type_ID = 1;
                            db.AdoptionPayments.Add(obj);
                            db.SaveChanges();
                        }
                    //Sending SMS
                    var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                    var authToken = "4186739dfb2554741e7dff014074ff82";
                    TwilioClient.Init(accountSid, authToken);
                    var message = MessageResource.Create(
                        to: new Twilio.Types.PhoneNumber("+27676367506"),
                        from: new Twilio.Types.PhoneNumber("+14245431153"),
                        body: "CONGRATULATION!" + " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + "" + "your Adoption Payment through a Cash payment was Successful! From Wollies Animal Shelter."

                    );
                }
                else if (Payment == "EFT")
                {
                    Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
                        if (aaa != null)
                        {
                            aaa.Adopt_Status_ID = 5;
                            //AdoptionPayment obj = db.AdoptionPayments.SingleOrDefault(x => x.Adoption_ID == id);
                            AdoptionPayment obj = new AdoptionPayment();
                            obj.Adoption_Fee = aaa.Animal.Animal_Type.Price;
                            obj.Adoption_ID = id;
                            obj.Animal_Type_ID = aaa.Animal.Animal_Type_ID;
                            obj.Payment_Type_ID = 2;
                            db.AdoptionPayments.Add(obj);
                            db.SaveChanges();
                        }

                    //Sending SMS
                    var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                    var authToken = "4186739dfb2554741e7dff014074ff82";
                    TwilioClient.Init(accountSid, authToken);
                    var message = MessageResource.Create(
                        to: new Twilio.Types.PhoneNumber("+27676367506"),
                        from: new Twilio.Types.PhoneNumber("+14245431153"),
                        body: "CONGRATULATION!"+ " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname+""+  "your Adoption Payment through an EFT payemnet was Successful! From Wollies Animal Shelter."
                    );
                }
               
                TempData["PaymentMessage"] = "Payment Successfully";
                db.SaveChanges();
               
            }
            Id = Convert.ToInt32(id);
            if (id == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["PaymentMessage"] = "Payment Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View("Index");
        }
        public ActionResult CollectAnimal(int? id)
        {
            try { 

            Adoption adoption = db.Adoptions.Find(id);
            
            ViewBag.ID = id;
            ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
            ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" + " " + adoption.Animal.Animal_Image;
            Id = Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                // return View("Error");
            }
            finally
            {

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CollectAnimal(int? id, DateTime? date)
        {
            try {
            var statusID = new List<Adoption>();
            var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).Include(a => a.AdoptionPayment).ToList();
            List<Adoption> adoption1 = db.Adoptions.ToList();
            Id = Convert.ToInt32(id);
            Adoption adoption = db.Adoptions.Find(id);//Display the animal details object
            
            if (adoption != null)
            {
                Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
                ViewBag.ID = id;
                ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" + " " + adoption.Animal.Animal_Image;
                aaa.Adopt_Status_ID = 6;
                DateTime dd = new DateTime();
                //    String year = date.Substring(0, 4);
                //   String month = date.Substring(5, 2);
                //    String day = date.Substring(8, 2);
              
                aaa.Collection_Date = date;
                db.SaveChanges();
                //Sending SMS
                var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                var authToken = "4186739dfb2554741e7dff014074ff82";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    to: new Twilio.Types.PhoneNumber("+27676367506"),
                    from: new Twilio.Types.PhoneNumber("+14245431153"),
                    body: "CONGRATULATION!"+ " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname+""+ "You have Successfully Collected your Adopted Animal from Wollies Animal Shelter!"
                );
                return View("Finalise");
            }
            Id = Convert.ToInt32(id);
            if (id == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
              
            }
            finally
            {

            }
            ///TempData["CollectMessage"] = "Animal Successfully Collected";
            return View("Finalise");
        }
        public ActionResult Finalise(int? id)
        {
            try {//id = Id;
            Adoption adoption = db.Adoptions.Find(id);
             
            if (adoption != null)
            {
                ViewBag.ID = id;
                ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" + " " + adoption.Animal.Animal_Image;
                Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
                aaa.Adopt_Status_ID = 7;
                aaa.Animal.Animal_Status_ID = 4;
                TempData["FinaliseMessage"] = "CONGRATULATION!!" + " " + adoption.Animal.Animal_Name + " " + "Successfully Adopted by" + " " + adoption.Adopter.Adopter_Name;
                db.SaveChanges();
                //TempData["FinaliseMessage"] = "CONGRATULATION!!" + " " + adoption.Animal.Animal_Name + " " + "Successfully Adopted by" + " " + adoption.Adopter.Adopter_Name;
                }
            
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return Redirect("http://localhost:55003/Adoptions/Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finalise(string name  ="", int id=0)
        {
            try {

             id = Id;
            Adoption adoption = db.Adoptions.Find(id);
             
            if (adoption != null)
            {
                ViewBag.ID = id;
                ViewBag.IDName = adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname + ", " + adoption.Adopter.Adopter_Email;
                ViewBag.IDet = adoption.Animal.Animal_Name + ", " + adoption.Animal.Animal_Type.Animal_Type_Name + ", " + adoption.Animal.Animal_Breed.Animal_Breed_Name.ToString() + ", " + adoption.Animal.Animal_Age.ToString() + " Years old" ;
                ViewBag.AnimalImage = adoption.Animal.Animal_Image;
                Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
                aaa.Adopt_Status_ID = 7;
                aaa.Animal.Animal_Status_ID = 4;
                TempData["FinaliseMessage"] = "CONGRATULATION!!" + " " + adoption.Animal.Animal_Name + " " + "Successfully Adopted by" + " " + adoption.Adopter.Adopter_Name;
                db.SaveChanges();

                //Sending SMS
                var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                var authToken = "4186739dfb2554741e7dff014074ff82";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    to: new Twilio.Types.PhoneNumber("+27676367506"),
                    from: new Twilio.Types.PhoneNumber("+14245431153"),
                    body: "CONGRATULATION!"+ " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname+ "you have Successfully Adopted" + " " + adoption.Animal.Animal_Name + " " + "from Wollies Animal Shelter. Please Come again!");
            }
            TempData["FinaliseMessage"] ="CONGRATULATION!!"+" "+ adoption.Animal.Animal_Name+" "+ "Successfully Adopted by" + " " +adoption.Adopter.Adopter_Name;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return Redirect("http://localhost:55003/Adoptions/Index");
        }
        public ActionResult ReturnAnimal(int? id)
        {
            try
            {
                Adoption adoption = db.Adoptions.Find(id);
                Adoption aaa = db.Adoptions.FirstOrDefault(x => x.Adoption_ID == id);
            aaa.Adopt_Status_ID = 8;
            aaa.Animal.Animal_Status_ID = 2;
          
                db.SaveChanges();

            //Sending SMS
            var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
            var authToken = "4186739dfb2554741e7dff014074ff82";
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                to: new Twilio.Types.PhoneNumber("+27676367506"),
                from: new Twilio.Types.PhoneNumber("+14245431153"),
                body: "SADLY!"+ " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname +""+  "the Animal" + " " + adoption.Animal.Animal_Name + " " + "Was unfortunaley returned to Wollies Animal Shelter by you! In doing so Your Adoption HAS been TERMINATED!");

            TempData["ReturnMessage"] = "SADDLY!!" + " " + adoption.Animal.Animal_Name + " " + "WAS RETURNED BY ADOPTER:" + " " + adoption.Adopter.Adopter_Name;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return Redirect("http://localhost:55003/Adoptions/Index");

        }
        public ActionResult ReturnIndex()
        {

            var statusID = new List<Adoption>();
            var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).Include(a => a.AdoptionPayment).ToList();
            try { 

            foreach (var item in adoptions)
            {
                if (item.Adopt_Status_ID == 7)
                {
                    statusID.Add(item);
                }
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View(statusID);
        }

        public ActionResult HomeCheckIndex(/*string searchBy, string search*/)
        {
            
            var statusID = new List<Adoption>();
            
            var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).Include(a => a.AdoptionPayment).ToList();
            try { 
            
            foreach (var item in adoptions)
            {
                if (item.Adopt_Status_ID == 1 || item.Adopt_Status_ID == 4)
                {
                    statusID.Add(item);
                }
            }
                return View(statusID);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            //try
            //{
            //    if (searchBy == "Animal_Name")
            //        return View(db.Adoptions.Where(x => x.Animal.Animal_Name == search || search == null).ToList());
            //    else
            //        return View(db.Adoptions.Where(x => x.Adopter.Adopter_Name == search || search == null).ToList());
            //}
            //catch (Exception err)
            //{
            //    ViewBag.err = err.Message;
            //}
            return View(statusID);

        }

        public ActionResult AdoptionPayemenHistory()
        {
            var statusID = new List<Adoption>();
            var adoptions = db.AdoptionPayments.Include(a => a.Adoption.Adopter).Include(a => a.Adoption.Animal).Include(a => a.Adoption.AdoptionPayments).ToList();
            try { 
            foreach (var item in adoptions)
            {
                if (item.Adoption.Adopt_Status_ID == 5)
                {
                    //statusID.Add(item);
                }
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                // return View("Error");
            }
            finally
            {

            }
            return View(statusID);
        }

        public ActionResult HomeCheckHistory()
        {
            var statusID = new List<Adoption>();
            var adoptions = db.HomeChecks.Include(a => a.Adoption).Include(a => a.Adoption.Animal).Include(a => a.Adoption.Adopter).ToList();
            return View(statusID);
        }

        public ActionResult HomeCheckReportHistory()
        {
            try {
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }

            return View();
        }

        public ActionResult AdoptionPaymentIndex()
        {

            var statusID = new List<Adoption>();
            var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).Include(a => a.AdoptionPayment).ToList();

            try { 
            foreach (var item in adoptions)
            {
                if (item.Adopt_Status_ID == 3)
                {
                    statusID.Add(item);
                }
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                // return View("Error");
            }
            finally
            {

            }
            return View(statusID);
        }
        public ActionResult CollectionIndex()
        {
            var statusID = new List<Adoption>();
            var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).Include(a => a.AdoptionPayment).ToList();

            try { 
            foreach (var item in adoptions)
            {
                if (item.Adopt_Status_ID == 5 || item.Adopt_Status_ID == 6)
                {
                    statusID.Add(item);
                }
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View(statusID);
        }
        public ActionResult HomeCheckReportIndex()
        {
            var statusID = new List<Adoption>();
            var adoptions = db.Adoptions.Include(a => a.Adopter).Include(a => a.Animal).ToList();

            try { 
            foreach (var item in adoptions)
            {
                if (item.Adopt_Status_ID == 2)
                {
                    statusID.Add(item);
                }
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View(statusID);
        }


        


        
        // GET: Adoptions/Create
        public ActionResult Create()
        {
            //db.Database.CommandTimeout = 300; 
            var statusID = new List<Animal>();
            var adoptions = db.Animals.ToList();
            try
            {
                animalsss = db.Animals.ToList();

            foreach (Animal item in animalsss)
            {
                if (item.Animal_Status_ID == 2)
                {
                    statusID.Add(item);
                }

            }
            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", "Adopter_Surname");
            ViewBag.Animal_ID = new SelectList(statusID, "Animal_ID", "Animal_Name", "Animal_Type");
            ViewBag.Payment_ID = new SelectList(db.Payments, "Payment_ID", "Payment_Description");
            ViewBag.Adopt_Status_ID = new SelectList(db.Adoption_Status, "Adopt_Status_ID", "Adopt_Status_Name");
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View();
        }

        // POST: Adoptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Adoption_ID,Adoption_Date,Adoption_Form,Payment_ID,Adopter_ID,Adopt_Status_ID,Animal_ID,Collection_Date")] Adoption adoption, string ADate = "")
        {
           
            var statusID = new List<Animal>();
            var adoptions = db.Animals.ToList();
            try
            {
                if (ModelState.IsValid)
            {
                String year = ADate.Substring(0, 4);
                String month = ADate.Substring(5, 2);
                String day = ADate.Substring(8, 2);
                DateTime dd = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                adoption.Adoption_Date = dd;
                db.Adoptions.Add(adoption);
               
               
               
                 animalsss = db.Animals.ToList();

                foreach (Animal item in animalsss)
                {
                    if (item.Animal_Status_ID == 2)
                    {
                        statusID.Add(item);
                    }

                }


                adoption.Animal.Animal_Status_ID = 3;
                db.SaveChanges();

                //Sending SMS
                var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                var authToken = "4186739dfb2554741e7dff014074ff82";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    to: new Twilio.Types.PhoneNumber("+27676367506"),
                    from: new Twilio.Types.PhoneNumber("+14245431153"),
                    body: "CONGRATULATION!"+ " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname+ "You have Successfully Started the Adoption Process with" + " " + adoption.Animal.Animal_Name + " " +"On the date of"+" "+ ADate + " "+   "!From Wollies Animal Shelter."
                );

                TempData["AdoptionCreateMessage"] = "Adoption Process Successfully Created";
                return RedirectToAction("Index");
            }

            ViewBag.Adopter_ID = new SelectList(db.Adopters, "Adopter_ID", "Adopter_Name", adoption.Adopter_ID);
            ViewBag.Animal_ID = new SelectList(statusID, "Animal_ID", "Animal_Name", adoption.Animal_ID);
            //ViewBag.Payment_ID = new SelectList(db.Payments, "Payment_ID", "Payment_Description", adoption.Payment_ID);
            ViewBag.Adopt_Status_ID = new SelectList(db.Adoption_Status, "Adopt_Status_ID", "Adopt_Status_Name", adoption.Adopt_Status_ID);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View(adoption);
        }

        
        public ActionResult Delete(int? id)
        {
            try { 
            Id = Convert.ToInt32(id);

            if (id == null)
            {
                throw new Exception("Something Went Wrong!");
            }
            Adoption adoption = db.Adoptions.Find(id);
            if (adoption == null)
            {
                    throw new Exception("Something Went Wrong!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return View(adoption);
        }

        // POST: Adoptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {
                List<Adoption> n = new List<Adoption>();
                Adoption adoption1 = db.Adoptions.Find(Id);
                adoption1.Animal.Animal_Status_ID = 2;
                

                db.Adoptions.Remove(adoption1);
            
                TempData["AdoptionDeleteMessage"] = "Adoption Process Successfully Deleted";
                
                db.SaveChanges();

                //Sending SMS
                var accountSid = "AC4b74118b2030829577ecb11b15da7bc9";
                var authToken = "4186739dfb2554741e7dff014074ff82";
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    to: new Twilio.Types.PhoneNumber("+27676367506"),
                    from: new Twilio.Types.PhoneNumber("+14245431153"),
                    body: "Unfortunately!" + " " + adoption.Adopter.Title.Title_Description + " " + adoption.Adopter.Adopter_Name + " " + adoption.Adopter.Adopter_Surname+ " "+ " Adoption Process has been Cancelled. From Wollies!"
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong!");
                //return View("Error");
            }
            finally
            {

            }
            return RedirectToAction("Index");
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ScheduleHomeCheckCalendar()
        {

            return Redirect("http://localhost:55003/Adoptions/Index");
        }
        public JsonResult GetEvents()
        {
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                var events = dc.Event_Schedule.ToList();
                var adoption = dc.Adoptions.ToList();
                ViewBag.Adoption = adoption;
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event_Schedule e)
        {
            var status = false;
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Event_Schedule.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Adoption.Adopter.Adopter_Name = e.Adoption.Adopter.Adopter_Name + " " + v.Adoption.Adopter.Adopter_Name +" " + v.Adoption.Adopter.Adopter_Surname;
                        v.Employee.Emp_Name = e.Employee.Emp_Name + " " + v.Employee.Emp_Surname;
                        v.Start = e.Start;
                        v.EventEnd = e.EventEnd;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColour = e.ThemeColour;
                    }

                }
                else //Add Event
                {
                    dc.Event_Schedule.Add(e);

                }
                TempData["HomeCheckReportMessage"] = "HomeCheck Successfully Scheduled";
                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                var v = dc.Event_Schedule.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Event_Schedule.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        
    }
}
