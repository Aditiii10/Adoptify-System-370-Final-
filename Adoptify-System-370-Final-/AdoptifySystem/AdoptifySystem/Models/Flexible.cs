using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.Models
{
    public class Flexible
    {
        public List<Employee> employeelist { get; set; }
        public Employee employee { get; set; }
        public Adopter AdoptAdopter { get; set; }
        public Adopter_Relative ARelative { get; set; }
        
    }
}