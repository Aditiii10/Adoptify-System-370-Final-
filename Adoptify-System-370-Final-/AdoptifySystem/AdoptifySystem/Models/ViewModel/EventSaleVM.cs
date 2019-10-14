using AdoptifySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class EventSaleVM
    {
        //Events for the month
        public string Event_Name { get; set; }
        public decimal Event_Ticket_Price { get; set; }
        public DateTime Event_StartDate { get; set; }
        public IEnumerable<Event_> Event { get; set; }
        public IEnumerable<Customer_Event> CustomerEvent { get; set;}
        public IEnumerable<EventCus> EventCus { get; set; }


        //Calculate
        public int TicketAvailable { get; set; }

        //TABLE Customer_Event
        public int Number_of_tickets { get; set; }
    }
}