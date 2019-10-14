using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using AdoptifySystem.Models;
using AdoptifySystem.Models.ViewModel;

namespace AdoptifySystem.Controllers
{
    public class EventsController : Controller
    {
        public Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: Events
        public ActionResult Index()
        {
          return View();
        }

        public JsonResult GetEvents()
        {
            var Events = (from even in db.Event_
                          where even.Event_Status == "pending"
                          select new
                          {
                              Date = even.Event_Date.ToString(),
                              Name = even.Event_Name.ToString(),
                              Price = even.Event_Ticket_Price.ToString(),
                              Description = even.Event_Description.ToString(),
                              Tickets = even.TicketAvailable.ToString(),
                              ID = even.Event_ID.ToString()
                          }).ToList();

            return new JsonResult { Data =  Events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [WebMethod]
        [HttpPost]
        public JsonResult CreateEvent(string Name ,string Surname , string Email , string Amount ,string Total , string Description , string Type, string Event, Payment payment , Customer_Event events)
        {
            string status = "false";
            //find Payment Type ID using type // 
            var paymentTypeID = db.Payment_Type.Where(b => b.Payment_Type_Name == Type).FirstOrDefault();
            //Get The Date//
            DateTime Today = DateTime.Today;
             using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                payment.Payment_Description = Description;
                payment.Amount_Paid = Convert.ToDecimal(Total);
                payment.Payment_Date = Today;
                payment.Payment_Type_ID = paymentTypeID.Payment_Type_ID;
                db.Payments.Add(payment);
                db.SaveChanges();

                events.Customer_Event_Name = Name;
                events.Customer_Event_Surname = Surname;
                events.Customer_Event_Email = Email;
                events.Number_of_tickects = Convert.ToInt32(Amount);
                events.TicketFee_Total = Convert.ToInt32(Total);
                events.TicketFee_Date = Today;
                events.Event_ID = Convert.ToInt32(Event);
                events.Payment_ID = payment.Payment_ID;
                events.Payment_Type_ID = paymentTypeID.Payment_Type_ID;
                db.Customer_Event.Add(events);
                db.SaveChanges();
                status = "True";
                
                
            }
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [WebMethod]
        [HttpPost]

        public JsonResult ViewEvent(int Name)
        {
            using (Wollies_ShelterEntities db = new Wollies_ShelterEntities())
            {
                List<Events> List = db.Customer_Event.Where(x => x.Event_ID == Name).Select(x => new Events {/* eID = Convert.ToInt32(x.Event_ID)*/ ADate = x.TicketFee_Date, CName = x.Customer_Event_Name, SName = x.Customer_Event_Surname, EEmail = x.Customer_Event_Email, TTicket = x.Number_of_tickects.ToString(), TTotal = x.TicketFee_Total.ToString() }).ToList();
               // List<string> eventID = List.Select(x => x.eID.ToString()).ToList();
                List<string> ADate = List.Select(x => x.ADate.ToShortDateString()).ToList();
                List<string> CName = List.Select(x => x.CName).ToList();
                List<string> SName = List.Select(x => x.SName.ToString()).ToList();
                List<string> EEmail = List.Select(x => x.CName.ToString()).ToList();
                List<string> TTicket = List.Select(x => x.TTicket.ToString()).ToList();
                List<string> TTotal = List.Select(x => x.TTotal.ToString()).ToList();
                var GrandTotal = db.Customer_Event.Where(x => x.Event_ID == Name).Sum(x => x.TicketFee_Total);
                db.ReduceTicketsAvailable(Convert.ToInt32(Name));

                return Json(new { Date = ADate, CusName = CName, Surname = SName, Email = EEmail, Tickets = TTicket, Total = TTotal , GrandTotal }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}