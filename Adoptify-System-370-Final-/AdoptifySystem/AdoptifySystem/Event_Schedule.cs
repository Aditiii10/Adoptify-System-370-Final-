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
    
    public partial class Event_Schedule
    {
        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> EventEnd { get; set; }
        public string ThemeColour { get; set; }
        public Nullable<bool> IsFullDay { get; set; }
    }
}
