using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using OfficeOpenXml;

namespace AdoptifySystem.Controllers.Cassie
{
    public class Animal_Kennel_HistoryController : Controller
    {

        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Innovation inno = new Innovation();
        // GET: Animal_Kennel_History
        public ActionResult Index()
        {
            return View();
        }

        // GET: Animal_Kennel_History/Create
        public ActionResult Create()
        {

            try
            {
                db.Database.CommandTimeout = 300;
                List<Kennel> kennels = new List<Kennel>();
                List<Animal> animals = new List<Animal>();


                List<Kennel> listy = new List<Kennel>();

                animals = db.Animals.ToList();
                kennels = db.Kennels.ToList();
                if (animals == null)
                {
                    return RedirectToAction("Index");
                }
                if (kennels == null)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in kennels)
                {
                    if (item.Animals.Count > 0)
                    {


                        if (item.Animals.Count <= item.Kennel_Capacity)
                        {
                            listy.Add(item);

                        }
                    }
                    else
                    {
                        listy.Add(item);
                    }
                }
                inno.Kennels = listy;
                inno.animals = animals;
                return View(inno);
            }
            catch (Exception e)
            {
                string ex = e.Message;
                throw new Exception("Something went Wrong");
            }

        }
        [HttpGet]
        public JsonResult Search_Kennel(int inid)
        {
            try
            {
                int id = Convert.ToInt32(inid);
                inno.Kennel = inno.Kennels.Where(z => z.Kennel_ID == id).FirstOrDefault();
                var kennels = inno.Kennels.Select(x => new
                {
                    Kennel_ID = x.Kennel_ID,
                    Kennel_Name = x.Kennel_Name,
                    Kennel_Space = (x.Kennel_Capacity - x.Animals.Count),
                    Kennel_Capacity = x.Kennel_Capacity,
                }).ToList();
                var kennel = kennels.Where(z => z.Kennel_ID == id).FirstOrDefault();
                ViewBag.breed = kennels;
                return Json(kennel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new Exception("Something went Wrong");
            }

        }
        [HttpGet]
        public ActionResult Search_Animal(int inid)
        {
            try
            {
                int id = Convert.ToInt32(inid);
                inno.animal = inno.animals.Where(z => z.Animal_ID == id).FirstOrDefault();
                return View("Create", inno);
            }
            catch (Exception)
            {

                throw new Exception("Something went Wrong");
            }

        }
        public ActionResult Moveanimal(int inid)
        {
            try
            {
                if (inno.Kennel == null || inno.animal == null)
                {

                    return View("Create", inno);
                }
                if (inno.animal.Kennel_ID != 0)
                {
                    if (inno.Kennel.Kennel_ID == inno.animal.Kennel_ID)
                    {
                        return View("Create", inno);
                    }
                    var animal = db.Animals.Find(inno.animal.Animal_ID);
                    if (animal == null)
                    {
                        return View("Create", inno);
                    }
                    animal.Kennel_ID = inno.Kennel.Kennel_ID;
                    db.SaveChanges();

                }

                return View("Create", inno);
            }
            catch (Exception)
            {

                throw new Exception("Something went Wrong");
            }

        }
    }
}