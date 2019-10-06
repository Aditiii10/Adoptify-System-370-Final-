using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class FosterCarePeriodsController : Controller
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        // GET: FosterCareDates
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddFosterCareDates()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {

                throw;
            }
            
        }
        [HttpPost]
        public ActionResult AddFosterCareDates(FosterCareDuration time)
        {
            try
            {
                if (time == null)
                {
                    return View();
                }
                else
                {
                    
                   
                        db.FosterCareDurations.Add(time);
                        db.SaveChanges();
                    
                }
                return View();
            }
            catch (Exception e)
            {

                throw;
            }

        }
        [HttpPost]
        public ActionResult MaintainFosterCareDates(int id)
        {
            try
            {
                FosterCareDuration fd = db.FosterCareDurations.Where(z=>z.FosterCareDuration_Id == id).FirstOrDefault();
                if (fd == null)
                {
                    return RedirectToAction("SearchFosterCareDates");
                }
                
                return View(fd);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult MaintainFosterCareDates(FosterCareDuration durations,string button)
        {
            try
            {
                if (button == "Save")
                {
                    List<FosterCareDuration> Foste = new List<FosterCareDuration>();
                    Foste = db.FosterCareDurations.ToList();
                    FosterCareDuration foste = db.FosterCareDurations.Find(durations.FosterCareDuration_Id);
                    if (Foste.Count != 0)
                    {
                        int count = 0;
                        foreach (var item in Foste)
                        {
                            if (item.DurationTime == foste.DurationTime && item.FosterCareDuration_Id != foste.FosterCareDuration_Id)
                            {
                                count++;
                                ViewBag.errorMessage = "There is a duplicate User Role Already";
                                return View(item);
                            }

                        }
                        if (count == 0)
                        {
                            foste.DurationTime = durations.DurationTime;
                            db.SaveChanges();
                        }
                        return RedirectToAction("SearchFosterCareDates");
                    }
                    return RedirectToAction("SearchFosterCareDates");
                }
                else if (button == "Cancel")
                {
                    return RedirectToAction("SearchFosterCareDates");
                }
                return RedirectToAction("SearchFosterCareDates");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult SearchFosterCareDates()
        {
            try
            {
                List<FosterCareDuration> Dates = new List<FosterCareDuration>();
                using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
                {
                    Dates = db.FosterCareDurations.ToList();

                }
                    return View(Dates);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult SearchFosterCareDates(int search)
        {
            try
            {
                List<FosterCareDuration> Dates = new List<FosterCareDuration>();
                using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
                {
                    Dates = db.FosterCareDurations.Where(z=> z.DurationTime == search).ToList();
                }
                return View(Dates);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}