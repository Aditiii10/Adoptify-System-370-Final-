using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class VetAppVM
    {
        public DateTime AppointmentDate { get; set; }
        public string Animal_Name { get; set; }
        public string Vet_Name { get; set; }
        public string Vet_Tel { get; set; }
        public string Kennel_Name { get; set; }
    }
}