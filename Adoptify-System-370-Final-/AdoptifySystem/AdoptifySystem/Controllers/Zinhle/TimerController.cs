using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class TimerController : Controller
    {
        // GET: Timer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddTimer()
        {
            return View();
        }
        public ActionResult DeleteTimer()
        {
            return View();
        }
        public ActionResult MaintainTimer()
        {
            return View();

        }
    }
}