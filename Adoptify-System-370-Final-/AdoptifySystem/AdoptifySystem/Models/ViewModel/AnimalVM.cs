using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class AnimalVM
    {
        public DateTime Animal_Entry_Date { get; set; }
        public string Animal_Name { get; set; }
        public string Animal_Gender { get; set; }
        public int Animal_Age { get; set; }
        public bool Animal_Sterilization { get; set; }
        public bool Animal_Castration { get; set; }

        //Animal_Type_ID
        public string Animal_Type_Name { get; set; }

        //Animal_Status_ID
        public string Animal_Status_Name { get; set; }

        //Kennel_ID
        public string Kennel_Name { get; set; }
    }
}