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
    
    public partial class UserRole
    {
        public int UserRole_ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> Role_ID { get; set; }
    
        public virtual Role_ Role_ { get; set; }
        public virtual User_ User_ { get; set; }
    }
}
