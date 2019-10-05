using AdoptifySystem.Models.nickeymodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class KennelController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        public static Flexible flex = new Flexible();
        // GET: Kennel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddKennel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddKennel(Kennel kennel)
        {
            try
            {
                db.AddKennel(kennel.Kennel_Name, kennel.Kennel_Number, kennel.Kennel_Capacity);
                
                RedirectToAction("SearchKennel");
            }
            catch (Exception e)
            {

                throw new Exception("Something Went Wrong!");
            }
            return View();
        }
        //Message = "The data reader is incompatible with the specified 'Wollies_ShelterModel.Kennel'. A member of the type, 'Kennel_ID', does not have a corresponding column in the data reader with the same name."
        public ActionResult SearchKennel()
        {
            try
            {
                db.Database.CommandTimeout = 300;

                return View(db.Kennels.ToList()) ;
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }
            
        }

        public ActionResult MaintainKennel(int id)
        {

            try
            {
                
                Kennel kennel = db.Kennels.Find(id);
                if (kennel == null)
                {
                    RedirectToAction("SearchKennel");
                }
                return View(kennel);
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }

        }
        [HttpPost]
        public ActionResult MaintainKennel(Kennel kennel)
        {

            try
            {
                db.UpdateKennel(kennel.Kennel_ID,kennel.Kennel_Name, kennel.Kennel_Number, kennel.Kennel_Capacity);
                return RedirectToAction("SearchKennel");
            }
            catch (Exception e)
            {

                throw new Exception("Something Went Wrong!");
            }

        }
        public ActionResult DeleteKennel(int? id)
        {
            try
            {

                if (id != null)
                {
                    Kennel kennel = db.Kennels.Find(id);
                    int count = kennel.Animals.Count();
                    if (count != 0)
                    {
                        //you cant delete becasue its referenced to another table
                        ViewBag.err = "You can not delete this";
                        return RedirectToAction("SearchKennel");
                    }
                    else
                    {
                        //db.Kennels.Remove(kennel);
                        db.DeleteKennel(kennel.Kennel_ID);
                        db.SaveChanges();
                        return RedirectToAction("SearchKennel");
                    }
                }
                return RedirectToAction("SearchKennel");
            }
            catch (Exception)
            {

                throw new Exception("Something Went Wrong!");
            }


        }
        [HttpPost]
        public ActionResult SearchKennel(string search)
        {
            try
            {
                if (!(search == ""))
                {
                    db.Database.CommandTimeout = 300;
                    var kennel = db.Kennels.Where(z => z.Kennel_Name.StartsWith(search)).ToList();
                    if (kennel == null)
                    {
                        return RedirectToAction("SearchKennel");
                    }
                    List<Kennel> kennels = new List<Kennel>();
                    kennels = kennel;

                    return View("SearchKennel", kennels);

                }
                TempData["SuccessMessage"] = "Enter Valid Details";
                return RedirectToAction("SearchKennel");
            }
            catch (Exception)
            {
                throw new Exception("Something Wrong");
            }


        }

    }
}