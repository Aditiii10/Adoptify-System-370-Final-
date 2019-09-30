using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoptifySystem.ViewModel
{
    public class AuditTrailVM
    {
        //Report Criteria
        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        public int Auditlog_ID { get; set; }
        public DateTime Auditlog_DateTime { get; set; }
        public string Transaction_Type { get; set; }
        public string Critical_Date { get; set; }
        public int UserID { get; set; }

        public int Emp_ID { get; set; }

        public string Emp_Name { get; set; }
        public string Emp_Surname { get; set; }

        public int SelectedTransactionTypeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        //Report Data
        public Audit_Log auditTrail { get; set; }
        public User_ user { get; set; }
        public Employee employee { get; set; }
        public List<IGrouping<string, ReportRecordAudit>> results { get; set; }
    }
    public class ReportRecordAudit
    {
        public string Auditlog_DateTime { get; set; }
        public string Emp_Name { get; set; }
        public int UserID { get; set; }
        public string Transaction_Type { get; set; }
        public string Critical_Date { get; set; }

    }

}