using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;

namespace AdoptifySystem.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            //TempData["EmailSucessMessage"] = "Email Successfully Sent";
            //TempData["EmailErrorMessage"] = "Email Unseccessfully Sent";
            return View();
        }

        [HttpPost]

        public ActionResult SendEmailView()
        {
            //call SendEmailView view to invoke webmail  
            return View();
        }
    }
}