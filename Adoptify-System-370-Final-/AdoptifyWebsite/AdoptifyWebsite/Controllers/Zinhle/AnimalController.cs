using AdoptifyWebsite.Models;
using AdoptifyWebsite.Models.nickeymodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdoptifyWebsite.Controllers.Zinhle
{
    public class AnimalController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        static public Innovation innovation = new Innovation();

        // GET: Animal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchAnimal()
        {
            List<Animal> animals = new List<Animal>();
            try
            {
                db.Database.CommandTimeout = 300;
                animals = db.Animals.ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Something Went Wrong!");
                // TempData["EditMessage"] = e.Message;
                // return RedirectToAction("AddTemporaryAnimal", "Animal");
            }


            return View(animals);
        }

        [HttpPost]
        public ActionResult SearchAnimal(string search)
        {

            if (!(search == ""))
            {
                var animallist = db.Animals.Where(z => z.Animal_Name.Equals(search)).ToList();
                if (animallist == null)
                {
                    return RedirectToAction("SearchAnimal");
                }
                List<Animal> animals = new List<Animal>();
                animals = animallist;

                return View("SearchAnimal", animals);

            }
            TempData["SuccessMessage"] = "Enter Valid Details";
            return View();
        }
    }
}