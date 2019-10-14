using AdoptifySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class TimesheetVM
    {
        public DateTime Check_in { get; set; }
        public DateTime Check_out { get; set; }
    
        public string Emp_Name { get; set; }
        public string Emp_Surname { get; set; }
    }



 
}