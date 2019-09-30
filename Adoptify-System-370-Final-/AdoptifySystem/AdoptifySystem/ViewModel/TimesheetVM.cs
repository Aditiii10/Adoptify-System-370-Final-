using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.ViewModel
{
    public class TimesheetVM
    {
        //Report Criteria
        public IEnumerable<SelectListItem> EmployeeName { get; set; }
        public int SelectedEmployeeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Emp_ID { get; set; }
        public string Emp_Name { get; set; }
        public string Emp_Surname { get; set; }
        public string Emp_Gender { get; set; }
        public int TimeSheet_ID { get; set; }
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }

        public Employee employee { get; set; }
        public List<IGrouping<string, ReportRecordTimesheet>> results { get; set; }

    }
    public class ReportRecordTimesheet
    {

        public string Emp_Name { get; set; }
        public string Emp_Surname { get; set; }
        public int Emp_ID { get; set; }
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }

    }
}