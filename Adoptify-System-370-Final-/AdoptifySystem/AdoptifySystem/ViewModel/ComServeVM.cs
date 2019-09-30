using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.ViewModel
{
    public class ComServeVM
    {
        //Report Criteria
        public IEnumerable<SelectListItem> Volunteers { get; set; }
        public int SelectedVolunteerID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public int Vol_ID { get; set; }
        public string Vol_Name { get; set; }
        public string Vol_Surname { get; set; }
        public int Vol_Hours_ID { get; set; }
        public DateTime Vol_workDate { get; set; }
        public DateTime Vol_Start_Time { get; set; }
        public DateTime Vol_End_Time { get; set; }
        public int Vol_WorkType_ID { get; set; }
        public string Vol_WorkType_Name { get; set; }

        //Report Data
        public Volunteer volunteer { get; set; }
        public Volunteer_Hours volunteerHours{ get; set; }
        public Volunteer_Work_Type volunteerWorkType{ get; set; }
        public List<IGrouping<string, ReportRecordComServe>> results { get; set; }
    }
    public class ReportRecordComServe
    {
        public int Vol_ID { get; set; }
        public string Date { get; set; }
        public DateTime Vol_workDate { get; set; }
        public DateTime Vol_Start_Time { get; set; }
        public DateTime Vol_End_Time { get; set; }

    }
}