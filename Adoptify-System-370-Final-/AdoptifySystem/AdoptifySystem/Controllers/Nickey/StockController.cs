using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Flexible flex = new Flexible();
        static int sub = 4;
        public ActionResult AddStock()
        {
            List<Stock_Type> Stock_Types = new List<Stock_Type>();
            List<Packaging_Type> Packaging_Type = new List<Packaging_Type>();
            List<Unit_Type> unit_Types = new List<Unit_Type>();
            try
            {
                
             
                    Stock_Types = db.Stock_Type.ToList();
                Packaging_Type = db.Packaging_Type.ToList();
                unit_Types = db.Unit_Type.ToList();
                flex.Stock_Types = Stock_Types;
                flex.packaging_Types = Packaging_Type;
                flex.unit_Types = unit_Types;
              
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }


            return View(flex);
        }
        [HttpPost]
        public ActionResult AddStock(Stock stock, string button)
        {
            try
            {
                if (button == "Save")
                {
                    var searchstock = db.Stocks.Where(z => z.Packaging_Type_ID == stock.Packaging_Type_ID && z.Stock_Description == stock.Stock_Description && z.Unit_Type_ID == stock.Unit_Type_ID && z.Unit_number == stock.Unit_number).FirstOrDefault();
                    if (searchstock == null)
                    {
                        db.Stocks.Add(stock);
                        db.SaveChanges();
                    }
                    return RedirectToAction("AddStock");
                }
                if (button == "Cancel")
                {

                }
            }
            catch (Exception e)
            {

                ViewBag.err = e.Message;
                throw new Exception("Something Went Wrong!");
            }
            return View("Index", "Home");
        }
        public ActionResult SearchStock()
        {
            ViewBag.errormessage = "";
            List<Stock> stock = new List<Stock>();
            try
            {
                stock = db.Stocks.ToList();
                
               
                    if (stock.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(stock);
           
        }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpPost]
        public ActionResult SearchStock(string search)
        {
            if (search != null)
            {

                List<Stock> stock = new List<Stock>();
                try
                {
                    //stock = db.searchstock(search).ToList();
                    //donation_types = db.Donation_Type.Where(z => z.Donation_Type_Name.StartsWith(search)|| z.Donation_Type_Description.StartsWith(search) ).ToList();
                    if (stock.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View(stock);
                    }
                    return View(stock);
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
        public ActionResult MaintainStock(int? id)
        {
            try
            {
                
             
                    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<Stock_Type> Stock_Types = new List<Stock_Type>();
                List<Packaging_Type> Packaging_Type = new List<Packaging_Type>();
                List<Unit_Type> unit_Types = new List<Unit_Type>();

                Stock_Types = db.Stock_Type.ToList();
                Packaging_Type = db.Packaging_Type.ToList();
                unit_Types = db.Unit_Type.ToList();
                flex.Stock_Types = Stock_Types;
                flex.packaging_Types = Packaging_Type;
                flex.unit_Types = unit_Types;

                Stock stock_ = db.Stocks.Find(id);
                if (stock_ == null)
                {
                    return RedirectToAction("SearchStock", "Stock");
                }
                flex.stock = stock_;
                return View(flex);
              
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpPost]
        public ActionResult MaintainStock(Stock stock2)
        {
            try
            {
                Stock stock1 = db.Stocks.Find(stock2.Stock_ID);
                if (stock1 == null)
                {
                    return Content("");
                }
                else
                {
                    stock1.Stock_Quantity = stock2.Stock_Quantity;
                    stock1.Packaging_Type_ID = stock2.Packaging_Type_ID;
                    stock1.Stock_Type_ID = stock2.Stock_Type_ID;
                    stock1.Unit_Type_ID = stock2.Unit_Type_ID;
                    stock1.Unit_number = stock2.Unit_number;
                    stock1.Stock_Description = stock2.Stock_Description;
                    db.Entry(stock1).CurrentValues.SetValues(stock2);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw new Exception("Something Went Wrong!");
            }
            return Content("");

        }

        public ActionResult CaptureStockTake(int? id)
        {
            try
            {
                
                
                    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<Stock_Type> Stock_Types = new List<Stock_Type>();
                List<Packaging_Type> Packaging_Type = new List<Packaging_Type>();
                List<Unit_Type> unit_Types = new List<Unit_Type>();

                Stock_Types = db.Stock_Type.ToList();
                Packaging_Type = db.Packaging_Type.ToList();
                unit_Types = db.Unit_Type.ToList();
                flex.Stock_Types = Stock_Types;
                flex.packaging_Types = Packaging_Type;
                flex.unit_Types = unit_Types;

                Stock stock_ = db.Stocks.Find(id);
                if (stock_ == null)
                {
                    return HttpNotFound();
                }
                flex.stock = stock_;
                return View(flex);
           
        }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }
        }
        [HttpPost]
        public ActionResult CaptureStockTake(Stock stock, string button)
        {
            try
            {
                if (button == "Save")
                {
                    Stock oldstock = db.Stocks.Find(stock.Stock_ID);
                    Stock newstock = db.Stocks.Find(stock.Stock_ID);
                    if (oldstock == null)
                    {
                        ViewBag.err = "Error not found";
                        return RedirectToAction("SearchStock", "Stock");
                    }
                    if (!(oldstock.Stock_Quantity > stock.Stock_Quantity))
                    {
                        ViewBag.err = "Quantity is will be in negatives";
                        return View("CaptureStockTake", flex);
                    }
                    newstock.Stock_Quantity -= stock.Stock_Quantity;
                    db.Entry(oldstock).CurrentValues.SetValues(newstock);
                    db.SaveChanges();
                    return RedirectToAction("SearchStock", "Stock");
                }
                if (button == "Cancel")
                {
                    return RedirectToAction("SearchStock");
                }
                return RedirectToAction("SearchStock", "Stock");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
        }
        public ActionResult AddStockType()
        {
            return View();
        }
        [HttpPost]
        public ContentResult AddStockType(Stock_Type stock_type)
        {
            try
            {

                List<Stock_Type> stock_types = new List<Stock_Type>();
                stock_types = db.Stock_Type.ToList();
                if (stock_types.Count != 0)
                {
                    int count = 0;
                    foreach (var item in stock_types)
                    {
                        if (item.Stock_Type_Name == stock_type.Stock_Type_Name && item.Stock_Type_Description == stock_type.Stock_Type_Description)
                        {
                            count++;
                            TempData["error"] = "There is a duplicate Stock Type Already";
                            return Content("");
                        }

                    }
                    if (count == 0)
                    {
                        db.Stock_Type.Add(stock_type);
                        db.SaveChanges();
                    }
                }
                else
                {

                    db.Stock_Type.Add(stock_type);
                    db.SaveChanges();


                }
                //Session["Userid"] = stock_type.Stock_Type_ID;

            }
            catch (Exception e)
            {
                TempData["exception"] = "There was an Error with network please try again: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }

            return Content("");
        }
        public ActionResult ReceiveStock(int? id)
        {
            try
            {
                
                    if (id == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                List<Stock_Type> Stock_Types = new List<Stock_Type>();
                List<Packaging_Type> Packaging_Type = new List<Packaging_Type>();
                List<Unit_Type> unit_Types = new List<Unit_Type>();

                Stock_Types = db.Stock_Type.ToList();
                Packaging_Type = db.Packaging_Type.ToList();
                unit_Types = db.Unit_Type.ToList();
                flex.Stock_Types = Stock_Types;
                flex.packaging_Types = Packaging_Type;
                flex.unit_Types = unit_Types;

                Stock stock_ = db.Stocks.Find(id);
                if (stock_ == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                flex.stock = stock_;
                return View(flex);

            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }

        }
        [HttpPost]
        public ActionResult ReceiveStock(Stock stock, string button)
        {
            try
            {
                if (button == "Save")
                {
                    Stock oldstock = db.Stocks.Find(stock.Stock_ID);
                    Stock newstock = db.Stocks.Find(stock.Stock_ID);
                    if (oldstock == null)
                    {
                        throw new Exception("Something Went Wrong!");
                    }
                    else
                    {
                        int old_stock = Convert.ToInt32(oldstock.Stock_Quantity);
                        int added_stock = Convert.ToInt32(stock.Stock_Quantity);
                        int new_stock = old_stock + added_stock;
                        newstock.Stock_Quantity = new_stock;
                        db.Entry(oldstock).CurrentValues.SetValues(newstock);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (button == "Cancel")
                {
                    return RedirectToAction("searchstock");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
        }
        public ActionResult SearchStockType()
        {
            ViewBag.errormessage = "";
            List<Stock_Type> stock_Types = new List<Stock_Type>();

            try
            {
                
                
                    stock_Types = db.Stock_Type.ToList();
                if (stock_Types.Count == 0)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(stock_Types);
            
        }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                throw new Exception("Something Went Wrong!");
            }

        }
        [HttpPost]
        public ActionResult SearchStockType(string search)
        {

            if (search != null)
            {

                List<Stock_Type> stock_type = new List<Stock_Type>();
                try
                {
                    //stock_type = db.searchstocktype(search).ToList();

                    if (stock_type.Count == 0)
                    {
                        TempData["noresult"] = "No results found";
                        return View(stock_type);
                    }
                    return View(stock_type);
                }
                catch (Exception e)
                {
                    TempData["exception"] = "there was a network error: " + e.Message;
                    throw new Exception("Something Went Wrong!");
                }
            }
            return View();
        }
        public ActionResult MaintainStockType(int? id)
        {
            try
            {
                
           
                    if (id == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                Stock_Type stock_Type = db.Stock_Type.Find(id);
                if (stock_Type == null)
                {
                    throw new Exception("Something Went Wrong!");
                }
                return View(stock_Type);
              
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }

        }
        [HttpPost]
        public ContentResult MaintainStockType(Stock_Type stock_Type)
        {
            try
            {

                Stock_Type Stock_Type = db.Stock_Type.Find(stock_Type.Stock_Type_ID);
                if (Stock_Type == null)
                {
                    return Content("");
                }
                else
                {

                    Stock_Type.Stock_Type_Name = stock_Type.Stock_Type_Name;
                    Stock_Type.Stock_Type_Description = stock_Type.Stock_Type_Description;
                    db.SaveChanges();

                }
            }
            catch (Exception e)
            {
                TempData["exception"] = e.Message;
                throw new Exception("Something Went Wrong!");
            }
            return Content("");
        }

        public ActionResult Deletestock(int id)
        {
            try
            {
                
            
                    if (id != 0)
                {
                    Stock stoc = db.Stocks.Find(id);
                    int count = stoc.Donation_Line.Count();
                    if (count == 0)
                    {
                        //you cant delete becasue its referenced to another table
                        return View("SearchStock");
                    }
                    else
                    {
                        db.Stocks.Remove(stoc);
                        db.SaveChanges();
                        return View("Index", "Home");
                    }
                }
                //need to send message that cant send message back
                return View("SearchStock");
               
            }
            catch (Exception)
            {
                throw new Exception("Something Went Wrong!");
            }


        }
        public ActionResult Deletestocktype(int id)
        {
            
         
                if (id != 0)
            {
                Stock_Type stocky = db.Stock_Type.Find(id);
                int count = stocky.Stocks.Count();
                if (count == 0)
                {
                    //you cant delete becasue its referenced to another table
                    return View("Searchstocktype");
                }
                else
                {
                    db.Stock_Type.Remove(stocky);
                    db.SaveChanges();
                    return View("Index", "Home");
                }
            }
            //need to send message that cant send message back

            return View("Searchstocktype");
      

}

    }
}