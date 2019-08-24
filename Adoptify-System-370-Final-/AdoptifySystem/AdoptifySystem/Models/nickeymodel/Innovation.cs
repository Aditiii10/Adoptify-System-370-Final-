using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.Models.nickeymodel
{
    public class Innovation
    {
        public List<Employee_Type> empTypeList { get; set; }
        public List<Employee> empList { get; set; }

        public List<Title> Titles { get; set; }
    }
}