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
    
    public partial class Vet_Appointment_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vet_Appointment_Master()
        {
            this.Vet_Appointment = new HashSet<Vet_Appointment>();
        }
    
        public int Vet_Appoint_Line_ID { get; set; }
        public int Vet_ID { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public Nullable<int> Animal_ID { get; set; }
        public Nullable<int> VetAppReasonsID { get; set; }
    
        public virtual Animal Animal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vet_Appointment> Vet_Appointment { get; set; }
        public virtual Veterinarian Veterinarian { get; set; }
        public virtual VetAppReason VetAppReason { get; set; }
    }
}
