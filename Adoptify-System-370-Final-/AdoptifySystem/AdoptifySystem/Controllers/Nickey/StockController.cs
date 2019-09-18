﻿using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;

namespace AdoptifySystem.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Flexible flex = new Flexible();
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

                return RedirectToAction("Index", "Home");
            }


            return View(flex);
        }
        [HttpPost]
        public ActionResult AddStock(Stock stock,string button)
        {
            try
            {
                if (button=="Save")
                {
                    var searchstock = db.Stocks.Where(z=>z.Packaging_Type_ID == stock.Packaging_Type_ID && z.Stock_Description == stock.Stock_Description && z.Unit_Type_ID == stock.Unit_Type_ID && z.Unit_number == stock.Unit_number).FirstOrDefault();
                    if (searchstock==null)
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
                    return RedirectToAction("AddStock");
            }
            return View("Index","Home");
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

                }
                return View(stock);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
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
                    return View();
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

                throw;
            }
        }
        [HttpPost]
        public ActionResult MaintainStock(Stock stock2, string button)
        {
            if (button == "Save")
            {
                try
                {
                    Stock stock1 = db.Stocks.Find(stock2.Stock_ID);
                    if (stock1 == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        stock2.Stock_Quantity = stock1.Stock_Quantity; 
                        db.Entry(stock1).CurrentValues.SetValues(stock2);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.err = e.Message;
                    return RedirectToAction("MaintainStock", "Stock");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("SearchStock", "Stock");
            }
            return RedirectToAction("SearchStock", "Stock");
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

                throw;
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
                    if(!(oldstock.Stock_Quantity > stock.Stock_Quantity))
                    {
                        ViewBag.err = "Quantity is will be in negatives";
                        return View("CaptureStockTake",flex);
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

                throw;
            }
        }
        public ActionResult AddStockType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddStockType(Stock_Type stock_type,string button)
        {
            
            if (button == "Save")
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
                                return View();
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

                }
                catch (Exception e)
                {
                    TempData["exception"] = "There was an Error with network please try again: " + e.Message;
                    return View();
                }

            }
            else if (button == "Cancel")
            {

                TempData["success"] = "The process has been cancelled succesfully";
                return RedirectToAction("SearchStockType", "Stock");
            }
            TempData["success"] = "The Stock Type has been added Succesfully";
            return RedirectToAction("SearchStockType", "Stock");
        }
        public ActionResult ReceiveStock(int? id)
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

                throw;
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
                        return HttpNotFound();
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
                    return RedirectToAction("Index","Home");
                }
                if (button == "Cancel")
                {
                    return RedirectToAction("searchstock");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
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

                }
                return View(stock_Types);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
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
                    return View();
                }
            }
            return View();
        }
        public ActionResult MaintainStockType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock_Type stock_Type = db.Stock_Type.Find(id);
            if (stock_Type == null)
            {
                return HttpNotFound();
            }
            return View(stock_Type);
        }
        [HttpPost]
        public ActionResult MaintainStockType(Stock_Type stock_Type,string button)
        {
            if (button == "Save")
            {
                try
                {
                    
                    Stock_Type Stock_Type = db.Stock_Type.Find(stock_Type.Stock_Type_ID);
                    if (Stock_Type == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        
                            db.Entry(Stock_Type).CurrentValues.SetValues(stock_Type);
                            db.SaveChanges();
                        
                    }
                }
                catch (Exception e)
                {
                    TempData["exception"] = e.Message;
                    return RedirectToAction("MaintainStockType", "Stock");
                }
            }
            else if (button == "Cancel")
            {
                TempData["success"] = "The Maintain process has been cancelled succesfully";
                return RedirectToAction("SearchStockType", "Stock");
            }
            TempData["success"] = "The Stock Type has been maintained succesfully";
            return RedirectToAction("SearchStockType", "Stock");
        }

        public ActionResult Deletestock(Stock id)
        {

            if (id != null)
            {
                int count = id.Donation_Line.Count();
                if (count == 0)
                {
                    //you cant delete becasue its referenced to another table
                    return View("SearchUserRole");
                }
                else
                {
                    db.Stocks.Remove(id);
                    db.SaveChanges();
                    return View("Index", "Home");
                }
            }
            //need to send message that cant send message back
            return View("SearchUserRole");

        }
        public ActionResult Deletestocktype(Stock_Type id)
        {

            if (id != null)
            {
                int count = id.Stocks.Count();
                if (count == 0)
                {
                    //you cant delete becasue its referenced to another table
                    return View("Searchstocktype");
                }
                else
                {
                    db.Stock_Type.Remove(id);
                    db.SaveChanges();
                    return View("Index", "Home");
                }
            }
            //need to send message that cant send message back

            return View("Searchstocktype");

        }
        
    }
}