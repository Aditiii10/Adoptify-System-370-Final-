using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using AdoptifySystem.Models.nickeymodel;
using Newtonsoft.Json;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;


namespace AdoptifySystem.Controllers
{
    public class DonationsController : Controller
    {
        // GET: Donations
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Flexible flex = new Flexible();
        public static List<Donation_Line> donlist = new List<Donation_Line>();
        public ActionResult AddDonor()
        {
            List<Title> titles = new List<Title>();
            try
            {
                titles = db.Titles.ToList();
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            

            return View(titles);
        }
        [HttpPost]
        public ContentResult AddDonor(Donor donor,string button)
        {
            try
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
                            return Content("");
                        }

                    }
                    if (count == 0)
                    {
                        db.Donors.Add(donor);
                        db.SaveChanges();
                        flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donor");
                    }
                }
                else
                {
                    db.Donors.Add(donor);
                    db.SaveChanges();
                    flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donor");
                }
                return Content("");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
                
        }
        public ActionResult MaintainDonor(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                Donor donor = db.Donors.Find(id);
                if (donor == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                flex.Titles = db.Titles.ToList();
                flex.donor = donor;

                return View(flex);
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        [HttpPost]
        public ContentResult MaintainDonor(Donor donor)
        {
            
                try
                {
                    Donor Donor = db.Donors.Find(donor.Donor_ID);
                    if (Donor == null)
                    {
                        return Content("");
                    }
                    else
                    {
                    Donor.Donor_Name = donor.Donor_Name;
                    Donor.Donor_Surname = donor.Donor_Surname;
                    Donor.Donor_Email = donor.Donor_Email;
                    Donor.Title_ID = donor.Title_ID;
                    db.SaveChanges();
                    flex.UpdateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donor");
                }
                }
                catch (Exception)
                {
                throw new Exception("Something Went Wrong!");
            }
            return Content("");
        }
        public ActionResult AddDonation()
        {
            ViewBag.errormessage = "";
            
            try
            {
                flex.donor = null;
                flex.donation = null;
                flex.stock = null;
                flex.adddonationlist = null;
                flex.DonorList = db.Donors.ToList();
                flex.Stocklist = db.Stocks.ToList();
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                ViewBag.errormessage = "";
                throw new Exception("Something Went Wrong!");
            }
            return View(flex);
        }
        //[HttpPost]
        //public ActionResult Addmoneytolist(Donation_Line donation_line,string[] checkeds, string button)
        //{
        //    ViewBag.errormessage = "";
        //    List<Donation_Line> temp = new List<Donation_Line>();

        //    if (button == "Select Donor")
        //    {
        //        try
        //        {
        //            flex.donor = db.Donors.Find(donation_line.Donation.Donor.Donor_ID);
        //        }
        //        catch (Exception e)
        //        {
        //            ViewBag.err = e.Message;
        //            ViewBag.errormessage = "";
        //        }
        //        return RedirectToAction("AddDonation");
        //    }

        //    if (button == "Add Money")
        //    {
        //        try
        //        {
        //            flex.Stocklist = db.Stocks.ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewBag.err = e.Message;
        //            ViewBag.errormessage = "";
        //        }
        //        return RedirectToAction("AddDonation");
        //    }
        //    if (button == "Add Stock")
        //    {
        //        try
        //        {
        //            flex.Stocklist = db.Stocks.ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewBag.err = e.Message;
        //            ViewBag.errormessage = "";
        //        }
        //        return RedirectToAction("AddDonation");
        //    }
        //    if (button == "Save")
        //    {
        //        try
        //        {
        //            flex.Stocklist = db.Stocks.ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewBag.err = e.Message;
        //            ViewBag.errormessage = "";
        //        }
        //        return RedirectToAction("AddDonation");
        //    }
        //    if (button == "Cancel")
        //    {
        //        try
        //        {
        //            flex.Stocklist = db.Stocks.ToList();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewBag.err = e.Message;
        //            ViewBag.errormessage = "";
        //        }
        //        return RedirectToAction("AddDonation");
        //    }
        //    return RedirectToAction("AddDonation");
        //}
        //[HttpGet]
        //public ActionResult search_donor(string inid)
        //{
        //    if (inid == "")
        //    {
        //        TempData[""] = "Pick a Donor please"; 
        //        return View("AddDonation", flex);
        //    }
        //    int id = Convert.ToInt32(inid);
        //    id = id + 1;
        //    flex.donor = flex.DonorList.Where(a => a.Donor_ID == id).FirstOrDefault();

        //    if (flex.donor == null)
        //    {
        //        return RedirectToAction("SearchDonation");
        //    }
        //    ViewBag.Donorname = flex.donor.Donor_Name;
        //    ViewBag.Donorsurname = flex.donor.Donor_Surname;
        //    return View("AddDonation", flex);

        //}
        [HttpGet]
        public JsonResult search_donor(string inid)
        {
            //List<Animal_Breed> breeds = new List<Animal_Breed>();
            try
            {
                if (inid == "")
                {
                    TempData[""] = "Pick a Donor please";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                int id = Convert.ToInt32(inid);
                id = id - 1;
                flex.donor = flex.DonorList.ElementAt(id);
                if (flex.donor == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                Donor data = new Donor();
                data.Donor_Name = flex.donor.Donor_Name;
                data.Donor_Surname = flex.donor.Donor_Surname;

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        //public ActionResult search_stock(string inid)
        //{
        //    if (inid == "")
        //    {
        //        TempData[""] = "Pick a Stock please";
        //        return View("AddDonation", flex);
        //    }
        //    int id = Convert.ToInt32(inid);
        //    flex.stock = flex.Stocklist.ElementAt(id);

        //    if (flex.stock == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View("AddDonation", flex);

        //}
        [HttpGet]
        public JsonResult search_stock(string inid)
        {
            try
            {
                if (inid == "")
                {
                    TempData[""] = "Pick a Stock please";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                int id = Convert.ToInt32(inid) - 1;
                flex.stock = flex.Stocklist.ElementAt(id);

                if (flex.stock == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                Stock data = new Stock();
                data.Stock_Name = flex.stock.Stock_Name;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            

        }
        public ActionResult add_stock(string Donation_Quantity)
        {
            try
            {
                if (flex.donor == null)
                {
                    return View("AddDonation", flex);
                }
                if (Donation_Quantity == "")
                {
                    return View("AddDonation", flex);
                }
                Donation_Line dl = new Donation_Line();
                dl.Donation_Quantity = Convert.ToInt32(Donation_Quantity);
                dl.Stock = flex.stock;
                dl.Stock_ID = flex.stock.Stock_ID;
                var Donation_Type = db.Donation_Type.Where(z => z.Donation_Type_Name == "Stock").FirstOrDefault();
                if (Donation_Type == null)
                {

                    return View("AddDonation", flex);
                }
                dl.Donation_Type = Donation_Type;
                dl.Donation_Type_ID = Donation_Type.Donation_Type_ID;
                donlist.Add(dl);
                if (donlist == null)
                {
                    return View("AddDonation", flex);
                }

                flex.adddonationlist = donlist;
                return View("AddDonation", flex);
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
                // return View("Error");
            }
           

        }
        public ActionResult add_money(string Donation_Quantity)
        {
            try
            {
                if (flex.donor == null)
                {
                    return View("AddDonation", flex);
                }
                if (Donation_Quantity == "")
                {
                    return View("AddDonation", flex);
                }
                Donation_Line don = new Donation_Line();

                var Donation_Type = db.Donation_Type.Where(z => z.Donation_Type_Name == "Money").FirstOrDefault();
                don.Donation_Quantity = Convert.ToDecimal(Donation_Quantity);
                if (Donation_Type == null)
                {
                    return View("AddDonation", flex);
                }
                don.Donation_Type = Donation_Type;
                don.Donation_Type_ID = Donation_Type.Donation_Type_ID;
                donlist.Add(don);
                //flex.donor
                flex.adddonationlist = donlist;

                return View("AddDonation", flex);
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            

        }

        public ActionResult removefromlist(int? donationline)
        {
            try
            {
                if (donationline != null)
                {
                    int id = Convert.ToInt32(donationline);
                    donlist.RemoveAt(id);
                    flex.adddonationlist = donlist;
                    return View("AddDonation", flex);
                }
                else
                {
                    return View("AddDonation", flex);
                }
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        public ActionResult save(string button)
        {
            try
            {
                if (button == "Save")
                {
                    if (flex.adddonationlist.Count != 0)
                    {
                        Donation don = new Donation();
                        don.Donation_Date = DateTime.Now;
                        don.Donor_ID = flex.donor.Donor_ID;
                        db.Donations.Add(don);
                        db.SaveChanges();

                        var searchDonations = db.Donations.Where(z => z.Donation_ID == don.Donation_ID).FirstOrDefault();
                        var dunking = don;
                        if (searchDonations == null)
                        {
                            //error
                            return View("AddDonation", flex);
                        }
                        foreach (Donation_Line item in flex.adddonationlist)
                        {
                            item.Donation_ID = searchDonations.Donation_ID;
                            if (item.Stock_ID != null)
                            {
                                var searchstock = db.Stocks.Where(z => z.Stock_ID == item.Stock_ID).FirstOrDefault();
                                var old = db.Stocks.Where(z => z.Stock_ID == item.Stock_ID).FirstOrDefault();
                                if (searchstock == null)
                                {
                                    return View("AddDonation", flex);
                                }
                                searchstock.Stock_Quantity += Convert.ToInt32(item.Donation_Quantity);
                                db.Entry(old).CurrentValues.SetValues(searchstock);
                                db.SaveChanges();
                                item.Stock = null;
                                
                            }
                            Donation_Line mydonation = new Donation_Line();
                            mydonation = item;
                            mydonation.Donation_Type = null;
                            mydonation.Donation = null;
                            db.Donation_Line.Add(mydonation);
                            db.SaveChanges();
                            

                        }
                        flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donation");
                        flex.adddonationlist = null;
                        flex.donor = null;
                        flex.stock = null;
                        return RedirectToAction("SearchDonation");

                    }
                    else
                    {
                        return View("AddDonation", flex);
                    }
                }
                if (button == "Cancel")
                {
                    return RedirectToAction("SearchDonation");
                }
                return RedirectToAction("SearchDonation");
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw new Exception("Something Went Wrong!");
            }
            


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
                ViewBag.err = e.Message;
                ViewBag.errormessage = "there was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
          
        }
        [HttpPost]
        public ActionResult SearchDonor(string search)
        {
            if (search != null)
            {

                List<Donor> donor = new List<Donor>();
                try
                {
                    //donor = db.searchDonor(search).ToList();
                    //donor = db.Donors.Where(z => z.Donor_Name.StartsWith(search) || z.Donor_Surname.StartsWith(search) || z.Donor_Email.StartsWith(search)).ToList();
                    if (donor.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View("SearchDonor", donor);
                    }
                    return View("SearchDonor", donor);
                }
                catch (Exception e)
                {
                    ViewBag.err = "there was a network error: " + e.Message;
                    throw new Exception("Something Went Wrong!");
                }
            }
            else
            {

            }
            return View("SearchDonor");

        }
        public ActionResult SearchDonation()
        {
            List<Donation> donantion = new List<Donation>();
            try
            {
                donantion = db.Donations.ToList();
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw new Exception("Something Went Wrong!");
            }


            return View(donantion);
        }
        [HttpPost]
        public ActionResult SearchDonation(string search)
        {
            if (search != null)
            {

                List<Donation> donantion = new List<Donation>();
                try
                {
                    //donation_types = db.SearchDon(search).ToList();
                    donantion = db.Donations.Where(z => z.Donor.Donor_Name.StartsWith(search) || z.Donor.Donor_Surname.StartsWith(search) || z.Donor.Donor_Email.StartsWith(search)).ToList();
                    if (donantion.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View(donantion);
                    }
                    return View(donantion);
                }
                catch (Exception e)
                {
                    ViewBag.err = "there was a network error: " + e.Message;
                    throw new Exception("Something Went Wrong!");
                }
            }
            else
            {

            }

            return View();
        }

        public ActionResult AddDonationType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDonationType(Donation_Type donation_Type)
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
                                return RedirectToAction("AddDonationType");
                               // return Json("Failed");
                        }

                        }
                        if (count == 0)
                        {
                            db.Donation_Type.Add(donation_Type);
                            db.SaveChanges();
                        flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donation Type");
                    }
                    }
                    else
                    {

                        db.Donation_Type.Add(donation_Type);
                        db.SaveChanges();
                    //int id = Convert.ToInt32(Session["ID"].ToString());
                    // HttpContext.Current.Session["ID"];
                    flex.CreateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donation Type");

                }
                    
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "There was an Error with network please try again: "+e.Message;
                throw new Exception("Something Went Wrong!");
            }
            return RedirectToAction("SearchDonationType");

        }
       
        public ActionResult SearchDonationType()
        {
           
            List<Donation_Type> donation_Types = new List<Donation_Type>();
            try
            {
                donation_Types = db.Donation_Type.ToList();
                if(donation_Types.Count == 0)
                {
                    return RedirectToAction("Index","Home");
                }
                
                return View(donation_Types);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: "+ e.Message;
                throw new Exception("Something Went Wrong!");
            }
            
        }
        [HttpPost]
        public ActionResult SearchDonationType(string search)
        {
            if (search != null)
            {

                List<Donation_Type> donation_types = new List<Donation_Type>();
                try
                {
                    //donation_types = db.SearchDon(search).ToList();
                    //donation_types = db.Donation_Type.Where(z => z.Donation_Type_Name.StartsWith(search)|| z.Donation_Type_Description.StartsWith(search) ).ToList();
                    if (donation_types.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View("SearchDonationType", donation_types);
                    }
                    return View("SearchDonationType", donation_types);
                }
                catch (Exception e)
                {
                    ViewBag.err = "there was a network error: " + e.Message;
                    throw new Exception("Something Went Wrong!");
                }
            }
            else
            {

            }
            return View("SearchDonationType");
        }
        public ActionResult MaintainDonationType(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                Donation_Type donation_Type = db.Donation_Type.Find(id);
                if (donation_Type == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(donation_Type);
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        [HttpPost]
        public ActionResult MaintainDonationType(Donation_Type Donationtype)
        {
          try
                {
                    Donation_Type donation_Type = db.Donation_Type.Find(Donationtype.Donation_Type_ID);
                    if (donation_Type == null)
                    {
                    return Content("");
                    }
                    else
                    {
                    donation_Type.Donation_Type_Name = Donationtype.Donation_Type_Name;
                    donation_Type.Donation_Type_Description = Donationtype.Donation_Type_Description;
                    db.SaveChanges();
                    flex.UpdateAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donation Type");
                }
                }
                catch(Exception e)
                {
                    ViewBag.err = e.Message;
                throw new Exception("Something Went Wrong!");
            }
            return Content("");
        }
        public ActionResult DeleteDonor(int? id)
        {
            try
            {
                if (id != null)
                {
                    Donor donor = db.Donors.Find(id);
                    int count = donor.Donations.Count();
                    if (count != 0)
                    {
                        //you cant delete becasue its referenced to another table
                        ViewBag.err = "You can not delete this";
                        return RedirectToAction("SearchDonor");
                    }
                    else
                    {
                        db.Donors.Remove(donor);
                        db.SaveChanges();
                        flex.DeleteAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donor");
                        return RedirectToAction("SearchDonor");
                    }
                }
                return RedirectToAction("SearchDonor");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        public ActionResult DeleteDonationType(int? id)
            {
            try
            {

                if (id != null)
                {
                    Donation_Type donation_type = db.Donation_Type.Find(id);
                    int count = donation_type.Donation_Line.Count();
                    if (count != 0)
                    {
                        //you cant delete becasue its referenced to another table
                        ViewBag.err = "You can not delete this";
                        return RedirectToAction("SearchDonationType");
                    }
                    else
                    {
                        db.Donation_Type.Remove(donation_type);
                        db.SaveChanges();
                        flex.DeleteAuditTrail(Convert.ToInt32(Session["ID"].ToString()), "Donation Type");
                        return RedirectToAction("SearchDonationType");

                    }
                }
                return RedirectToAction("SearchDonationType");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }


            }
        public ActionResult CreateathankyoudocumentforDonors(string text)
        {
            try
            {
                using (WordDocument document = new WordDocument())
                {
                    List<Donor> donlist = db.Donors.ToList();
                    //Adds new section to the document
                    IWSection section = document.AddSection();
                    ////Adds new paragraph to the section
                    ////Adds line break to the paragraph
                    ////string torepalcae = "pageBreakPara";
                    //IWParagraph pageBreakPara = section.AddParagraph();
                    ////IWParagraph title = section.AddParagraph();
                    //pageBreakPara.AppendText("Dear ");
                    //pageBreakPara.AppendBreak(BreakType.LineBreak);
                    ////Adds page break to the paragraph'
                    //pageBreakPara.AppendText("This will be paragraph that we enter details");
                    //pageBreakPara.AppendBreak(BreakType.PageBreak);
                    //pageBreakPara.AppendText("After page break");
                    //int x = 0;

                    foreach (var item in donlist)
                    {
                        //string torepalcae = "pageBreakPara";
                        IWParagraph dunkin = section.AddParagraph();
                        //IWParagraph title = section.AddParagraph();
                        dunkin.AppendText("Dear " + item.Donor_Name);
                        dunkin.AppendBreak(BreakType.LineBreak);
                        //Adds page break to the paragraph'
                        dunkin.AppendText("This will be paragraph that we enter details");
                        dunkin.AppendBreak(BreakType.PageBreak);
                        dunkin.AppendText("After page break");

                    }
                    //Saves and closes the document instance
                    //Saves the Word document to disk in DOCX format
                    document.Save("Result.docx", FormatType.Docx, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                }
                return View();
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        public ActionResult UploadDonor(HttpPostedFileBase xmlfile)
        {
            try
            {
                if (xmlfile.ContentType.Equals("application/xml") || xmlfile.ContentType.Equals("text/xml"))
                {
                    var xmlPath = Server.MapPath("~/FileUpload" + xmlfile.FileName);
                    xmlfile.SaveAs(xmlPath);
                    List<Donor> customers = new List<Donor>();

                    //Load the XML file in XmlDocument.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);

                    //Loop through the selected Nodes.
                    foreach (XmlNode node in doc.SelectNodes("/Donors/Donor"))
                    {
                        //Fetch the Node values and assign it to Model.
                        customers.Add(new Donor
                        {
                            Donor_ID = int.Parse(node["Donor_ID"].InnerText),
                            Donor_Name = node["Donor_Name"].InnerText,
                            Donor_Surname = node["Donor_Surname"].InnerText,
                            Donor_Email = node["Donor_Email"].InnerText,
                            Title_ID = int.Parse(node["Title_ID"].InnerText),
                        });
                    }
                    using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
                    {
                        foreach (var i in customers)
                        {
                            var v = db.Donors.Where(a => a.Donor_ID.Equals(i.Donor_ID)).FirstOrDefault();

                            if (v != null)
                            {
                                v.Donor_ID = i.Donor_ID;
                                v.Donor_Name = i.Donor_Name;
                                v.Donor_Surname = i.Donor_Surname;
                                v.Donor_Email = i.Donor_Email;
                                v.Title_ID = i.Title_ID;
                            }
                            else
                            {
                                db.Donors.Add(i);
                            }
                            db.SaveChanges();
                        }
                    }
                }
                else
                {

                }
                return RedirectToAction("SearchDonor");
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }
            
        }

        public ActionResult export()
        {
            try
            {
                List<Donor> donlist = db.Donors.ToList();
                if (donlist.Count > 0)
                {
                    var xEle = new XElement("Donors",
                        from don in donlist
                        select new XElement("Donor",
                            new XElement("Donor_ID", don.Donor_ID),
                            new XElement("Donor_Name", don.Donor_Name),
                            new XElement("Donor_Surname", don.Donor_Surname),
                            new XElement("Donor_Email", don.Donor_Email),
                            new XElement("Title_ID", don.Title_ID),
                            new XElement("Donations", don.Donations)
                            ));
                    Response.Write(xEle);
                    Response.ContentType = "application/xml";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=DonorsList.xml");
                    Response.End();
                }
                return View("SearchDonor");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }
        public ActionResult ViewDonor(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                Donor donor = db.Donors.Find(id);
                if (donor == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                flex.Titles = db.Titles.ToList();
                flex.donor = donor;

                return View(flex);
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }
            
        }

    }
}