using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models.nickeymodel;
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
                else
                {
                    db.Donors.Add(donor);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index", "Home");
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
        [HttpPost]
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
                    ViewBag.err = e.Message;
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
        [HttpGet]
        public ActionResult search_donor(string inid)
        {
            if (inid == "")
            {
                TempData[""] = "Pick a Donor please"; 
                return View("AddDonation", flex);
            }
            int id = Convert.ToInt32(inid);
            id = id + 1;
            flex.donor = flex.DonorList.Where(a => a.Donor_ID == id).FirstOrDefault();

            if (flex.donor == null)
            {
                return RedirectToAction("SearchDonation");
            }
            ViewBag.Donorname = flex.donor.Donor_Name;
            ViewBag.Donorsurname = flex.donor.Donor_Surname;
            return View("AddDonation", flex);

        }
        public ActionResult search_stock(string inid)
        {
            if (inid == "")
            {
                TempData[""] = "Pick a Stock please";
                return View("AddDonation", flex);
            }
            int id = Convert.ToInt32(inid);
            flex.stock = flex.Stocklist.ElementAt(id);

            if (flex.stock == null)
            {
                return HttpNotFound();
            }

            return View("AddDonation", flex);

        }
        public ActionResult add_stock(string Donation_Quantity)
        {
            if (Donation_Quantity == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation_Line dl = new Donation_Line();
            dl.Donation_Quantity = Convert.ToInt32(Donation_Quantity);
            dl.Stock = flex.stock;
            dl.Stock_ID = flex.stock.Stock_ID;
            var Donation_Type = db.Donation_Type.Where(z=> z.Donation_Type_Name == "Stock").FirstOrDefault();
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
        public ActionResult add_money(string Donation_Quantity)
        {

            if (Donation_Quantity == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

        public ActionResult removefromlist(int? donationline)
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
                return View("AddDonation", flex);
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
                return View();
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
                    donor = db.searchDonor(search).ToList();
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
                    return View("SearchDonor");
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
                return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("SearchDonation");
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
        [HttpPost]
        public ActionResult SearchDonationType(string search)
        {
            if (search != null)
            {

                List<Donation_Type> donation_types = new List<Donation_Type>();
                try
                {
                    donation_types = db.SearchDon(search).ToList();
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
                    return View("SearchDonationType");
                }
            }
            else
            {

            }
            return View("SearchDonationType");
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
                    ViewBag.err = e.Message;
                    return RedirectToAction("MaintainDonationType","Donations");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DeleteDonor(int? id)
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
                    return RedirectToAction("SearchDonor");
                }
            }
            return RedirectToAction("SearchDonor");
        }
        public ActionResult DeleteDonationType(int? id)
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
                        return RedirectToAction("SearchDonationType");
                    }
                }
                return RedirectToAction("SearchDonationType");

            }
        public ActionResult CreateathankyoudocumentforDonors()
        {
          
                // Creating a new document.
                WordDocument document = new WordDocument();
                //Adding a new section to the document.
                WSection section = document.AddSection() as WSection;
                //Set Margin of the section
                section.PageSetup.Margins.All = 72;
                //Set page size of the section
                section.PageSetup.PageSize = new System.Drawing.SizeF(612, 792);

                //Create Paragraph styles
                WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11f;
                style.ParagraphFormat.BeforeSpacing = 0;
                style.ParagraphFormat.AfterSpacing = 8;
                style.ParagraphFormat.LineSpacing = 13.8f;

                style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
                style.ApplyBaseStyle("Normal");
                style.CharacterFormat.FontName = "Calibri Light";
                style.CharacterFormat.FontSize = 16f;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(46, 116, 181);
                style.ParagraphFormat.BeforeSpacing = 12;
                style.ParagraphFormat.AfterSpacing = 0;
                style.ParagraphFormat.Keep = true;
                style.ParagraphFormat.KeepFollow = true;
                style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
                IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();

                // Gets the image stream.
                IWPicture picture = paragraph.AppendPicture(new Bitmap("AdventureCycle.jpg")) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
                picture.VerticalOrigin = VerticalOrigin.Margin;
                picture.VerticalPosition = -45;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = 263.5f;
                picture.WidthScale = 20;
                picture.HeightScale = 15;

                paragraph.ApplyStyle("Normal");
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                WTextRange textRange = paragraph.AppendText("Adventure Works Cycles") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Calibri";
                textRange.CharacterFormat.TextColor = System.Drawing.Color.Red;

                //Appends paragraph.
                paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange = paragraph.AppendText("Adventure Works Cycles") as WTextRange;
                textRange.CharacterFormat.FontSize = 18f;
                textRange.CharacterFormat.FontName = "Calibri";

                //Appends paragraph.
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Adventure Works Cycles, the fictitious company on which the AdventureWorks sample databases are based, is a large, multinational manufacturing company. The company manufactures and sells metal and composite bicycles to North American, European and Asian commercial markets. While its base operation is in Bothell, Washington with 290 employees, several regional sales teams are located throughout their market base.") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                //Appends paragraph.
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("In 2000, AdventureWorks Cycles bought a small manufacturing plant, Importadores Neptuno, located in Mexico. Importadores Neptuno manufactures several critical subcomponents for the AdventureWorks Cycles product line. These subcomponents are shipped to the Bothell location for final product assembly. In 2001, Importadores Neptuno, became the sole manufacturer and distributor of the touring bicycle product group.") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                textRange = paragraph.AppendText("Product Overview") as WTextRange;
                textRange.CharacterFormat.FontSize = 16f;
                textRange.CharacterFormat.FontName = "Calibri";
                //Appends table.
                IWTable table = section.AddTable();
                table.ResetCells(3, 2);
                table.TableFormat.Borders.BorderType = BorderStyle.None;
                table.TableFormat.IsAutoResized = true;

                //Appends paragraph.
                paragraph = table[0, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                //Appends picture to the paragraph.
                picture = paragraph.AppendPicture(new Bitmap("Mountain-200.jpg")) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
                picture.VerticalOrigin = VerticalOrigin.Paragraph;
                picture.VerticalPosition = 4.5f;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = -2.15f;
                picture.WidthScale = 79;
                picture.HeightScale = 79;

                //Appends paragraph.
                paragraph = table[0, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.AppendText("Mountain-200");
                //Appends paragraph.
                paragraph = table[0, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                paragraph.BreakCharacterFormat.FontName = "Times New Roman";

                textRange = paragraph.AppendText("Product No: BK-M68B-38\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Size: 38\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Weight: 25\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Price: $2,294.99\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                //Appends paragraph.
                paragraph = table[0, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.AppendText("Mountain-300 ");
                //Appends paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                paragraph.BreakCharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Product No: BK-M47B-38\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Size: 35\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Weight: 22\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Price: $1,079.99\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                //Appends paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends paragraph.
                paragraph = table[1, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.LineSpacing = 12f;
                //Appends picture to the paragraph.
                picture = paragraph.AppendPicture(new Bitmap("Mountain-300.jpg")) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
                picture.VerticalOrigin = VerticalOrigin.Paragraph;
                picture.VerticalPosition = 8.2f;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = -14.95f;
                picture.WidthScale = 75;
                picture.HeightScale = 75;

                //Appends paragraph.
                paragraph = table[2, 0].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.LineSpacing = 12f;

                //Appends picture to the paragraph.
                picture = paragraph.AppendPicture(new Bitmap("Road-550-W.jpg")) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
                picture.VerticalOrigin = VerticalOrigin.Paragraph;
                picture.VerticalPosition = 3.75f;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = -5f;
                picture.WidthScale = 92;
                picture.HeightScale = 92;

                //Appends paragraph.
                paragraph = table[2, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.AppendText("Road-150 ");
                //Appends paragraph.
                paragraph = table[2, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                paragraph.BreakCharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Product No: BK-R93R-44\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Size: 44\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Weight: 14\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Price: $3,578.27\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                //Appends paragraph.
                section.AddParagraph();

                //Saves the Word document to disk in DOCX format
                document.Save("Sample.docx", FormatType.Docx, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
            return View();
        }
    }
}