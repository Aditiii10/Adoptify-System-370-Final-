using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nexmo.Api;
using AdoptifySystem.Models;
using System.Net;
using AdoptifySystem;

namespace AdoptifySystem.Controllers
{
    public class SMSMessageController : Controller
    {
        // GET: SMSMessage
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            Wollies_ShelterEntities db = new Wollies_ShelterEntities();

            var results = SMS.Send(new SMS.SMSRequest
            {
                from = Configuration.Instance.Settings["appsettings:NEXMO_FROM_NUMBER"],
                to = message.To,
                text = message.ContentMsg

            });

            TempData["AlertMessage"] = "SMS Succesfully Sent !!";
            return View();
        }
    }
}