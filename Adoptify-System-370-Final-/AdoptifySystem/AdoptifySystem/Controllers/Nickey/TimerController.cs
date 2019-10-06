using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class TimerController : Controller
    {
        // GET: Timer
        public ActionResult Index(User_ login)
        {
           
            return View();
        }

        public ActionResult AddTimer()
        {

            try
            {
                Timer time = new Timer();
                using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
                {
                    time = db.Timers.FirstOrDefault();
                }
                if (time == null)
                {
                    return View();
                }
                return View(time);
            }
            catch (Exception)
            {

                throw;
            }
            
            
            
        }
        [HttpPost]
        public ActionResult MaintainTimer(Timer timer)
        {
            try
            {
                Timer time = new Timer();
                using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
                {
                    time = db.Timers.FirstOrDefault();
                
                if (time == null)
                {

                        db.Timers.Add(timer);
                        db.SaveChanges();


                    }
                    else
                    {
                        time.Hours = timer.Hours;
                        time.Minutes = timer.Minutes;
                        time.Seconds = timer.Seconds;
                        db.SaveChanges();
                        
                    }
                    
                }
                return RedirectToAction("Index","Home");
            }
            catch (Exception)
            {

                throw;
            }
            

        }
        [HttpGet]
        public JsonResult getTime()
        {
            try
            {
                using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
                {
                    var time = db.Timers.Select(z => new
                    {
                        hours = z.Hours,
                        minutes = z.Minutes,
                        seconds = z.Seconds
                    }).FirstOrDefault();

                    return Json(time, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        
        }
    }
}