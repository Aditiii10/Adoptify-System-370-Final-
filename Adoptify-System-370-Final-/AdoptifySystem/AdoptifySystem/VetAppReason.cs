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
    
    public partial class VetAppReason
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VetAppReason()
        {
            this.Vet_Appointment = new HashSet<Vet_Appointment>();
            this.Vet_Appointment_Master = new HashSet<Vet_Appointment_Master>();
        }
    
        public int VetAppReasonsID { get; set; }
        public Nullable<int> Animal_Type_ID { get; set; }
        public string Reason { get; set; }
    
        public virtual Animal_Type Animal_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vet_Appointment> Vet_Appointment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vet_Appointment_Master> Vet_Appointment_Master { get; set; }
    }
}
