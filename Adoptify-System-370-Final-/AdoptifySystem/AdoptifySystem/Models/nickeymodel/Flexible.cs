
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.Models.nickeymodel
{
    public class Flexible
    {
        public List<Employee> employeelist { get; set; }
        public Employee employee { get; set; }

        public List<Packaging_Type> packaging_Types { get; set; }
        public List<Unit_Type> unit_Types { get; set; }
        public Donor donor { get; set; }
        public List<Title> Titles { get; set; }
        public List<Stock> Stocklist { get; set; }
        public List<Stock_Type> Stock_Types { get; set; }
        public List<Donation_Line> adddonationlist { get; set; }
    }
}