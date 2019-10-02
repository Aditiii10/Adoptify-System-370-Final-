using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.ViewModel
{
    public class AnimalDetailsVM
    {
        //Report Criteria
        public IEnumerable<SelectListItem> Animal_Names { get; set; }
        public IEnumerable<SelectListItem> Animal_Types { get; set; }
        public int Animal_ID { get; set; }
        public string Animal_Name { get; set; }
        public int Animal_Type_ID { get; set; }
        public string Animal_Type_Name { get; set; }
        public int Animal_Breed_ID { get; set; }
        public string Animal_Description { get; set; }
        public string Animal_Gender { get; set; }
        public int Animal_Age { get; set; }
        public bool Animal_Sterilization { get; set; }
        public bool Animal_Castration { get; set; }
        public System.DateTime Animal_Entry_Date { get; set; }
        public int Animal_Status_ID { get; set; }
        public string Animal_Status_Name { get; set; }
        public string Animal_Image_Name { get; set; }
        public string Animal_Image_Type { get; set; }
        public byte[] Animal_Image { get; set; }

        //Report Data
        public Animal animal { get; set; }
        public Animal_Type animalType { get; set; }
        public Animal_Status animalStatus { get; set; }
        public List<IGrouping<string, ReportRecord>> results { get; set; }

    }
    public class ReportRecord
    {
        public string Animal_Name { get; set; }
        public byte[] Animal_Image { get; set; }
        public string Animal_Type_Name { get; set; }
        public string Animal_Status_Name { get; set; }

    }
}