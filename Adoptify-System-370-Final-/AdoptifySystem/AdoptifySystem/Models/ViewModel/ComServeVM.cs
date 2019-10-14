using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class ComServeVM
    {
        public int Vol_ID { get; set; }
        public string Vol_Name { get; set; }
        public string Vol_Surname { get; set; }
        public DateTime Vol_workDate { get; set; }
        public TimeSpan Vol_Start_Time { get; set; }
        public TimeSpan Vol_End_Time { get; set; }

        public string Count { get; set; }
    }
}