using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using Flexible = AdoptifySystem.Models.nickeymodel.Flexible;

namespace AdoptifySystem.Controllers
{
    public class DonationsController : Controller
    {
        // GET: Donations
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Flexible flex = new Flexible(); 
        public ActionResult AddDonor()
        {
            List<Title> titles = new List<Title>();
            try
            {
                titles = db.Titles.ToList();
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Home");
            }
            

            return View(titles);
        }
        [HttpPost]
        public ActionResult AddDonor(Donor donor,string button)
        {
            if (button == "Save")
            {
                List<Donor> donors = new List<Donor>();
                donors = db.Donors.ToList();
                if (donors.Count != 0)
                {
                    int count = 0;
                    foreach (var item in donors)
                    {
                        if (item.Donor_Name == donor.Donor_Name && item.Donor_Surname == donor.Donor_Surname && item.Donor_Email == donor.Donor_Email)
                        {
                            count++;
                            ViewBag.errorMessage = "There is a duplicate Donor Already";
                            return View();
                        }

                    }
                    if (count == 0)
                    {
                        db.Donors.Add(donor);
                        db.SaveChanges();

                    }
                }
                db.Donors.Add(donor);
                db.SaveChanges();
                return View();
            }
            else if (button == "Cancel")
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        public ActionResult MaintainDonor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            flex.Titles = db.Titles.ToList();
            flex.donor = donor;
            
            return View(flex);
        }
        public ActionResult MaintainDonor(Donor donor,string button)
        {
            if (button == "Save")
            {
                try
                {
                    Donor Donor = db.Donors.Find(donor.Donor_ID);
                    if (Donor == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        db.Entry(Donor).CurrentValues.SetValues(donor);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("MaintainDonor", "Donations");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AddDonation()
        {
            ViewBag.errormessage = "";
            
            try
            {
                flex.Stocklist = db.Stocks.ToList();
            }
            catch (Exception e)
            {

                ViewBag.errormessage = "";
            }
            return View(flex);
        }
        [HttpPost]
        public ActionResult Addmoneytolist(Donation_Line donation_line,string[] checkeds, string button)
        {
            ViewBag.errormessage = "";
            List<Donation_Line> temp = new List<Donation_Line>();

            if (button == "Select Donor")
            {
                try
                {
                    flex.donor = db.Donors.Find(donation_line.Donation.Donor.Donor_ID);
                }
                catch (Exception e)
                {

                    ViewBag.errormessage = "";
                }
                return RedirectToAction("AddDonation");
            }

            if (button == "Add Money")
            {
                try
                {
                    flex.Stocklist = db.Stocks.ToList();
                }
                catch (Exception e)
                {

                    ViewBag.errormessage = "";
                }
                return RedirectToAction("AddDonation");
            }
            if (button == "Add Stock")
            {
                try
                {
                    flex.Stocklist = db.Stocks.ToList();
                }
                catch (Exception e)
                {

                    ViewBag.errormessage = "";
                }
                return RedirectToAction("AddDonation");
            }
            if (button == "Save")
            {
                try
                {
                    flex.Stocklist = db.Stocks.ToList();
                }
                catch (Exception e)
                {

                    ViewBag.errormessage = "";
                }
                return RedirectToAction("AddDonation");
            }
            if (button == "Cancel")
            {
                try
                {
                    flex.Stocklist = db.Stocks.ToList();
                }
                catch (Exception e)
                {

                    ViewBag.errormessage = "";
                }
                return RedirectToAction("AddDonation");
            }
            return RedirectToAction("AddDonation");
        }
        public ActionResult SearchDonor()
        {
            ViewBag.errormessage = "";
            List<Donor> donors = new List<Donor>();
            try
            {
                donors = db.Donors.ToList();
                if (donors.Count == 0)
                {

                }
                return View(donors);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
            }
          
        }
        public ActionResult SearchDonation()
        {
            List<Donation_Line> Donation_Lines = new List<Donation_Line>();
            try
            {
                Donation_Lines = db.Donation_Line.ToList();
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Home");
            }


            return View(Donation_Lines);
        }

        public ActionResult AddDonationType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDonationType(Donation_Type donation_Type, string button)
        {
            ViewBag.errorMessage = "";
            //Donation_Type asd = new Donation_Type();
            if (button == "Save")
            {
                try
                {

                    List<Donation_Type> donationtypes = new List<Donation_Type>();
                    donationtypes = db.Donation_Type.ToList();
                    if (donationtypes.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in donationtypes)
                        {
                            if (item.Donation_Type_Name == donation_Type.Donation_Type_Name)
                            {
                                count++;
                                ViewBag.errorMessage = "There is a duplicate Donation Type Already";
                                return View();
                            }

                        }
                        if (count == 0)
                        {
                            db.Donation_Type.Add(donation_Type);
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        db.Donation_Type.Add(donation_Type);
                        db.SaveChanges();
                       

                    }
                    
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "There was an Error with network please try again: "+e.Message;
                    return View();
                }
                
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public ActionResult SearchDonationType()
        {
            ViewBag.errormessage = "";
            List<Donation_Type> donation_Types = new List<Donation_Type>();
            try
            {
                donation_Types = db.Donation_Type.ToList();
                if(donation_Types.Count == 0)
                {

                }
                return View(donation_Types);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: "+ e.Message;
                return View();
            }
            
        }
        public ActionResult MaintainDonationType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation_Type donation_Type = db.Donation_Type.Find(id);
            if (donation_Type == null)
            {
                return HttpNotFound();
            }
            return View(donation_Type);
        }
        [HttpPost]
        public ActionResult MaintainDonationType(Donation_Type Donationtype,string button)
        {
            if (button == "Save")
            {
                try
                {
                    Donation_Type donation_Type = db.Donation_Type.Find(Donationtype.Donation_Type_ID);
                    if (donation_Type == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        db.Entry(donation_Type).CurrentValues.SetValues(Donationtype);
                        db.SaveChanges();
                    }
                }
                catch(Exception e)
                {
                    return RedirectToAction("MaintainDonationType","Donations");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}