﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.ViewModel
{
    public class AuditTrailVM
    {
        public int Auditlog_ID { get; set; }
        public DateTime Auditlog_DateTime { get; set; }
        public string Transaction_Type { get; set; }
        public string Critical_Date { get; set; }

        //User_ on UserID
        public string Username { get; set; }
    }
}