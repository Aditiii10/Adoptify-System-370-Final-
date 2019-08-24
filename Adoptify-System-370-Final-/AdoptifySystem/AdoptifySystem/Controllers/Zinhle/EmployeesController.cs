using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem.Models;
using AdoptifySystem.Models.nickeymodel;
using Flexible = AdoptifySystem.Models.nickeymodel.Innovation;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class EmployeesController : Controller
    {
        // GET: Donations
        Wollies_ShelterEntities1 db = new Wollies_ShelterEntities1();
        public static Innovation innovation = new Innovation();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddEmployee()
        {
         
            innovation.Titles = db.Titles.ToList();
            //innovation.empTypes = db.empTypes.ToList();
            return View(innovation);
        }

    }
}