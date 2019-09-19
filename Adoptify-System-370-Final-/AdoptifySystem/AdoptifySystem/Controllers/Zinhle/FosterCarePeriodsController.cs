using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class FosterCarePeriodsController : Controller
    {
        // GET: FosterCareDates
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddFosterCareDates()
        {
            return View();
        }

        public ActionResult MaintainFosterCareDates()
        {
            return View();
        }

        public ActionResult SearchFosterCareDates()
        {
            return View();
        }
    }
}