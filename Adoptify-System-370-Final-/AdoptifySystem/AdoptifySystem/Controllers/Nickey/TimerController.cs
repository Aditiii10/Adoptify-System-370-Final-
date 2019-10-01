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