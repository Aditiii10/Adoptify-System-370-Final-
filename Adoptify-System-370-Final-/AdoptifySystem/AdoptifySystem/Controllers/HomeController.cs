﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AdoptifySystem;
using System.Web.Script.Services;
using System.Web.Services;
using AdoptifySystem.Models.nickeymodel;



namespace AdoptifySystem.Controllers
{
   
    public class HomeController : Controller
    {
        private Wollies_ShelterEntities dc = new Wollies_ShelterEntities();
        // GET: Home
        public static Flexible flex = new Flexible();
        static int sub = 16;
       
        public ActionResult Index()
        {
            
                Wollies_ShelterEntities dc = new Wollies_ShelterEntities();
            try { 
            var AnimalsDogs = dc.Animals.Where(x => x.Animal_Type.Animal_Type_ID == 1 && x.Animal_Status.Animal_Status_ID == 2).Count();
            ViewBag.AnimalsDogs = AnimalsDogs;
            var AnimalsCats = dc.Animals.Where(x => x.Animal_Type.Animal_Type_ID == 2 && x.Animal_Status.Animal_Status_ID == 2).Count();
            ViewBag.AnimalsCats = AnimalsCats;
            var Adoptions = dc.Adoptions.Where(x => x.Adoption_Status.Adopt_Status_ID ==1 || x.Adoption_Status.Adopt_Status_ID==2 || x.Adoption_Status.Adopt_Status_ID == 3 || x.Adoption_Status.Adopt_Status_ID == 4 || x.Adoption_Status.Adopt_Status_ID == 5).Count();
            ViewBag.Adoptions = Adoptions;
            var Adopters = dc.Adopters.Count();
            ViewBag.Adopters = Adopters;
            var Employees = dc.Employees.Count();
            ViewBag.Employees = Employees;
            var Kennels = dc.Kennels.Count();
            ViewBag.Kennels = Kennels;
            }
            catch (Exception err)
            {
                throw new Exception("Something Went Wrong!");
            }
            
            return View();
        }
        
        public JsonResult GetEvents()
        {
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                var events = dc.Event_Schedule.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        
        [HttpPost]
        public JsonResult SaveEvent(Event_Schedule e)
        {
            var status = false;
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Event_Schedule.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.EventEnd = e.EventEnd;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColour = e.ThemeColour;
                    }
                    
                }
                else //Add Event
                {
                    dc.Event_Schedule.Add(e);
                    
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }
        
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                var v = dc.Event_Schedule.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Event_Schedule.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
      
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartData()
        {
            List<GoogleChartData> data = new List<GoogleChartData>();
            //Here MyDatabaseEntities  is our dbContext
            using (Wollies_ShelterEntities dc = new Wollies_ShelterEntities())
            {
                data = dc.GoogleChartDatas.ToList();
            }

            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]{
                "Year",
                "Cats",
                "Dogs",
                "Average"
            };

            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] {i.Year.ToString(), i.Cats, i.Dogs,
                    (i.Cats + i.Dogs)/2};
            }
            return chartData;
        }
    }
}