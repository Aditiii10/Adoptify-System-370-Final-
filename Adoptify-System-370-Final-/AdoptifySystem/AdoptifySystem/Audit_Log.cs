//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdoptifySystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Audit_Log
    {
        public int Auditlog_ID { get; set; }
        public System.DateTime Auditlog_DateTime { get; set; }
        public string Transaction_Type { get; set; }
        public string Critical_Date { get; set; }
        public int UserID { get; set; }
    
        public virtual User_ User_ { get; set; }
    }
}
