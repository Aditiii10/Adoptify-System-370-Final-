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
    
    public partial class Foster_Care
    {
        public int Foster_Care_ID { get; set; }
        public string Foster_Care_Period { get; set; }
        public Nullable<System.DateTime> Foster_Start_Date { get; set; }
        public Nullable<int> Foster_Parent_ID { get; set; }
        public Nullable<int> Animal_ID { get; set; }
        public Nullable<int> FosterCareDuration_Id { get; set; }
    
        public virtual Animal Animal { get; set; }
        public virtual Foster_Care_Parent Foster_Care_Parent { get; set; }
        public virtual FosterCareDuration FosterCareDuration { get; set; }
    }
}
