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
                    List<Foster_Care_Parent> parentlist = db.Foster_Care_Parent.ToList();
                    if (foster_Care_Parent1 == null)
                    {

                        return View(foster_Care_Parent);
                    }
                    else
                    {
                        foreach (var item in parentlist)
                        {
                            if (item == foster_Care_Parent1)
                            {

                            }
                            else if (item.Foster_Parent_Name == foster_Care_Parent.Foster_Parent_Name
                                   || item.Foster_Parent_Surname == foster_Care_Parent.Foster_Parent_Surname
                                   || item.Foster_Parent_Email == foster_Care_Parent.Foster_Parent_Email
                                  || item.Foster_Parent_CellNumber == foster_Care_Parent.Foster_Parent_CellNumber
                                  || item.Foster_Parent_IDNumber == foster_Care_Parent.Foster_Parent_IDNumber
                                  || item.Foster_Parent_WorkNumber == foster_Care_Parent.Foster_Parent_WorkNumber
                                  || item.Foster_Parent_Address == foster_Care_Parent.Foster_Parent_Address)
                            {
                                ViewBag.err = "There is a duplicate Fostercare parent already";
                                return View(foster_Care_Parent);
                            }
                        }
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
        [HttpPost]
        public ActionResult SearchFosterCareParent(string search)
        {
            if (search != null)
            {

                List<Foster_Care_Parent> foster = new List<Foster_Care_Parent>();
                try
                {
                    // foster = db.searchParent(search).ToList();
                    //foster = db.Foster_Care_Parent.Where(z => z.Foster_Parent_Email.StartsWith(search) || z.Donor_Surname.StartsWith(search) || z.Donor_Email.StartsWith(search)).ToList();
                    if (foster.Count == 0)
                    {
                        ViewBag.err = "No results found";
                        return View(foster);
                    }
                    return View(foster);
                }
                catch (Exception e)
                {
                    ViewBag.err = "there was a network error: " + e.Message;
                    return View();
                }
            }
            else
            {

                //}
                return View();
            }
        }
        public ActionResult AddtoFosterCare()
        {
            try
            {
                flex.Fostercarelist = null;
                flex.fostercareparent = db.Foster_Care_Parent.ToList();
                flex.animallist = db.Animals.Where(z => z.Animal_Status.Animal_Status_Name == "Available").ToList();
                return View(flex);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return RedirectToAction("Index", "home");
            }

        }

        public ActionResult search_parent(string inid)
        {
            if (inid == "")
            {
                ViewBag.parenterr = "Please search a animal";
                return View("AddtoFosterCare", flex);
            }
            int id = Convert.ToInt32(inid);
            id = id + 1;
            flex.parent = flex.fostercareparent.Where(a => a.Foster_Parent_ID == id).FirstOrDefault();

            if (flex.parent == null)
            {
                ViewBag.parenterr = "Please search a animal";
                return View("AddtoFosterCare", flex);
            }

            return View("AddtoFosterCare", flex);

        }

        public ActionResult search_animal(string inid)
        {
            if (inid == "")
            {
                ViewBag.animalerr = "Please search a animal";
                return View("AddtoFosterCare", flex);
            }
            int id = Convert.ToInt32(inid);
            flex.animal = flex.animallist.ElementAt(id);

            if (flex.animal == null)
            {
                ViewBag.animalerr = "Please search a animal";
                return View("AddtoFosterCare", flex);
            }

            return View("AddtoFosterCare", flex);

        }

        public ActionResult add(Foster_Care infoster)
        {

            try
            {
                if (flex.parent == null)
                {
                    ViewBag.parenterr = "Please search a parent";
                    return RedirectToAction("AddtoFosterCare");
                }
                if (flex.animal == null)
                {
                    ViewBag.animalerr = "Please search a animal";
                    return RedirectToAction("AddtoFosterCare");
                }
                Foster_Care foster1 = new Foster_Care();
                foster1.Animal_ID = flex.animal.Animal_ID;
                foster1.Animal = flex.animal;
                foster1.Foster_Care_Parent = flex.parent;
                foster1.Foster_Parent_ID = flex.parent.Foster_Parent_ID;
                foster1.Foster_Care_Period = infoster.Foster_Care_Period;
                foster1.Foster_Start_Date = infoster.Foster_Start_Date;

                test.Add(foster1);

                flex.Fostercarelist = test;
                int co = 0;
                foreach (var item in flex.animallist)
                {

                    if (item.Animal_ID == flex.animal.Animal_ID)
                    {
                        flex.animallist.RemoveAt(co);
                        flex.animal = null;
                        break;
                    }
                    co++;
                }

            }
            catch (Exception e)
            {
                var em = e.Message;
                return RedirectToAction("AddtoFosterCare");
            }
            return View("AddtoFosterCare", flex);

        }
        public ActionResult removefromlist(int? animalid)
        {

            if (animalid != null)
            {
                Foster_Care test = flex.Fostercarelist.Where(n => n.Animal_ID == animalid).FirstOrDefault();
                if (test == null)
                {
                    return RedirectToAction("AddtoFosterCare", flex);
                }
                flex.animallist.Add(test.Animal);
                flex.Fostercarelist.Remove(test);
                return View("AddtoFosterCare", flex);
            }
            else
            {
                return RedirectToAction("AddtoFosterCare", flex);
            }


        }

        public ActionResult savetofostercare()
        {
            try
            {
                foreach (var item in flex.Fostercarelist)
                {
                    //addthe foster care 
                    item.Animal = null;
                    item.Foster_Care_Parent = null;
                    db.Foster_Care.Add(item);
                    //change animal status to FOster Care
                    var orginal = db.Animals.Where(n => n.Animal_ID == item.Animal_ID).FirstOrDefault();
                    var chaghedstatus = db.Animals.Where(n => n.Animal_ID == item.Animal_ID).FirstOrDefault();
                    chaghedstatus.Animal_Status_ID = 2;
                    db.Entry(orginal).CurrentValues.SetValues(chaghedstatus);
                    db.SaveChanges();


                }
                flex.Fostercarelist = null;
                flex.fostercareparent = null;
                flex.animal = null;

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewBag.err = "complete all the details: " + e.Message;
                return View("AddtoFosterCare", flex);
                throw;
            }
        }
        public ActionResult RemovefromFosterCare()
        {
            try
            {
                List<Foster_Care> list = new List<Foster_Care>();
                list = db.Foster_Care.Where(z => z.Animal.Animal_Status.Animal_Status_Name == "Foster Care").ToList();
                if (list == null)
                {
                    ViewBag.err = "There are no Animals in Foster Care";
                    return RedirectToAction("Index", "Home");
                }
                flex.Fostercarelist = list;
                return View(flex);

            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                throw;
            }

        }
        public ActionResult searchfostercare(string search)
        {
            try
            {
                if (search != "")
                {
                    flex.Fostercarelist = flex.Fostercarelist.Where(z => z.Foster_Care_Parent.Foster_Parent_Name.Equals(search)).ToList();
                    return View("RemovefromFosterCare", flex);
                }
                return RedirectToAction("RemovefromFosterCare", flex);
            }
            catch (Exception e)
            {
                ViewBag.err = e.Message;
                return View("RemovefromFosterCare", flex);
            }

        }

        public ActionResult saveremovefostercare()
        {

            flex.Fostercarelist = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DeleteParent(int? id)
        {

            if (id != null)
            {
                Foster_Care_Parent parent = db.Foster_Care_Parent.Find(id);
                int count = parent.Foster_Care.Count();
                if (count != 0)
                {
                    //you cant delete becasue its referenced to another table
                    ViewBag.err = "You can not delete this";
                    return View("SearchFosterCareParent");
                }
                else
                {
                    db.Foster_Care_Parent.Remove(parent);
                    db.SaveChanges();
                    return View("SearchFosterCareParent");
                }
            }
            return View("SearchFosterCareParent");
        }
    }
}