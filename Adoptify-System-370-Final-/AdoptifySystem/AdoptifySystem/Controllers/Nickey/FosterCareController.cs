﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;

namespace AdoptifySystem.Controllers
{
    public class FosterCareController : Controller
    {
        static List<Foster_Care> test = new List<Foster_Care>();
        // GET: FosterCare
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        static Flexible flex = new Flexible();
        // GET: FosterCare
        public ActionResult AddFosterCareParent()
        {
            List<Foster_Care_Parent> mylist = new List<Foster_Care_Parent>();
            mylist = db.Foster_Care_Parent.ToList();
            return View(mylist);
        }
        [HttpPost]
        public ActionResult AddFosterCareParent(Foster_Care_Parent foster_Care_Parent, string button)
        {
            ViewBag.errorMessage = "";
            //Donation_Type asd = new Donation_Type();
            if (button == "Save")
            {
                try
                {

                    List<Foster_Care_Parent> foster_Care_Parents = new List<Foster_Care_Parent>();
                    foster_Care_Parents = db.Foster_Care_Parent.ToList();
                    

                    if (foster_Care_Parents.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in foster_Care_Parents)
                        {
                            if (item.Foster_Parent_Name == foster_Care_Parent.Foster_Parent_Name)
                            {
                                count++;
                                ViewBag.errorMessage = "There is a duplicate Donation Type Already";
                                return View();
                            }

                        }
                        if (count == 0)
                        {
                            db.Foster_Care_Parent.Add(foster_Care_Parent);
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        db.Foster_Care_Parent.Add(foster_Care_Parent);
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
        [HttpPost]
        public ActionResult MaintainFosterCareParent(Foster_Care_Parent foster_Care_Parent, string button)
        {
            if (button == "Save")
            {
                try
                {
                    Foster_Care_Parent foster_Care_Parent1 = db.Foster_Care_Parent.Find(foster_Care_Parent.Foster_Parent_ID);
                    if (foster_Care_Parent1 == null)
                    {

                        return HttpNotFound();
                    }
                    else
                    {
                        db.Entry(foster_Care_Parent1).CurrentValues.SetValues(foster_Care_Parent);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.err = e.Message;
                    return RedirectToAction("MaintainFosterCareParent", "Stock");
                }
            }
            else if (button == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MaintainFosterCareParent(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Foster_Care_Parent foster_Care_Parent = db.Foster_Care_Parent.Find(id);
                if (foster_Care_Parent == null)
                {
                    return HttpNotFound();
                }
                return View(foster_Care_Parent);

            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw;
            }
            
        }
        public ActionResult SearchFosterCareParent()
        {
            ViewBag.errormessage = "";
            List<Foster_Care_Parent> foster_Care_Parents = new List<Foster_Care_Parent>();
            try
            {
                foster_Care_Parents = db.Foster_Care_Parent.ToList();
                if (foster_Care_Parents.Count == 0)
                {

                }
                return View(foster_Care_Parents);
            }
            catch (Exception e)
            {
                ViewBag.errormessage = "there was a network error: " + e.Message;
                return View();
            }
        }
        public ActionResult AddtoFosterCare()
        {
            try
            {
                flex.fostercareparent = db.Foster_Care_Parent.ToList();
                flex.animallist = db.Animals.Where(z => z.Animal_Status.Animal_Status_Name == "Available").ToList();
                return View(flex);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("index", "home");
            }
            
        }
        public ActionResult search_parent(string inid)
        {
            if (inid == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = Convert.ToInt32(inid);
            id = id +1;
            flex.parent = flex.fostercareparent.Where(a => a.Foster_Parent_ID == id).FirstOrDefault();

            if (flex.parent == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("AddtoFosterCare", flex);

        }
        public ActionResult search_animal(string inid)
        {
            if (inid == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = Convert.ToInt32(inid);
            flex.animal = flex.animallist.ElementAt(id);

            if (flex.animal == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("AddtoFosterCare", flex);

        }
        static int co = 0;
        public ActionResult add(Foster_Care infoster)
        {
            
            try
            {
                if (flex.parent == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (flex.animal == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                co++;
                dynamic foster = new ExpandoObject();
                foster.Foster_Care_ID = co;
                foster.Animal_ID = flex.animal.Animal_ID;
                foster.Animal = flex.animal;
                foster.Foster_Care_Parent = flex.parent;
                foster.Foster_Parent_ID = flex.parent.Foster_Parent_ID;
                foster.Foster_Care_Period = infoster.Foster_Care_Period;
                foster.Foster_Start_Date = infoster.Foster_Start_Date;
                Foster_Care foster1 = new Foster_Care();
                foster1.Foster_Care_ID = co;
                foster1.Animal_ID = flex.animal.Animal_ID;
                foster1.Animal = flex.animal;
                foster1.Foster_Care_Parent = flex.parent;
                foster1.Foster_Parent_ID = flex.parent.Foster_Parent_ID;
                foster1.Foster_Care_Period = infoster.Foster_Care_Period;
                foster1.Foster_Start_Date = infoster.Foster_Start_Date;

       
                test.Add(foster1);
                //if (ModelState.IsValid)
                //{
                //    db.Foster_Care.Add(foster);
                //    db.SaveChanges();
                //}


                flex.Fostercarelist = test;

            }
            catch (Exception e)
            {
                var em = e.Message;
                return RedirectToAction("AddtoFosterCare");
            }
            
            

            return RedirectToAction("AddtoFosterCare", flex);

        }
        public ActionResult RemovefromFosterCare()
        {
            return View();
        }
    }
}