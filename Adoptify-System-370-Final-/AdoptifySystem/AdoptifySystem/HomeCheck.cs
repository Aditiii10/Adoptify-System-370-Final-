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
    
    public partial class HomeCheck
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HomeCheck()
        {
            this.HomeCheckReports = new HashSet<HomeCheckReport>();
        }
    
        public int HomeCheck_ID { get; set; }
        public Nullable<System.DateTime> HomeCheck_Datetime { get; set; }
        public Nullable<int> Adoption_ID { get; set; }
    
        public virtual Adoption Adoption { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeCheckReport> HomeCheckReports { get; set; }
    }
}
