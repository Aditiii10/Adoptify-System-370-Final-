//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdoptifyWebsite
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnimalTypePrice
    {
        public int AnimalTypePrice_ID { get; set; }
        public int Animal_Type_ID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Animal_Type Animal_Type { get; set; }
    }
}
