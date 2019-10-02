using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class IntakeReportVM
    {
        internal object Animal;

        //report criteria
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }


        //Fields for report data
        public Animal animal { get; set; }
        public List<IGrouping<string, ReportRecordAnimal>> results { get; set; }
        public Dictionary<string, double> chartData { get; set; }
    }
    public class ReportRecordAnimal
    {
        public string Animal_Entry_Date { get; set; }
        public double Animal_Age { get; set; }
        public string Animal_Gender { get; set; }
        public string Animal_Name { get; set; }
        public int Animal_Type_ID { get; set; }
        public string Animal_Type_Name { get; set; }
        public string Animal_Status_ID { get; set; }
        public string Animal_Status_Name { get; set; }
        public int Animal_ID { get; set; }
    }
}