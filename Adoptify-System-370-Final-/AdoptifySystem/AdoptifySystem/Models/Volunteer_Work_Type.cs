//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdoptifySystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;  //needed for Display annotation
    using System.ComponentModel;  //DisplayName annotation

    public partial class Volunteer_Work_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Volunteer_Work_Type()
        {
            this.Volunteer_Hours = new HashSet<Volunteer_Hours>();
        }
    
        [Display(Name = "Volunteer Work Type")]
        public int Vol_WorkType_ID { get; set; }


        [Display(Name = "Volunteer Work Type Name")]
        [Required]
        public string Vol_WorkType_Name { get; set; }


        [Display(Name = "Volunteer Work Type Description")]
        [Required]
        public string Vol_WorkType_Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volunteer_Hours> Volunteer_Hours { get; set; }
    }
}