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
    
    public partial class Animal_Breed
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Animal_Breed()
        {
            this.Animals = new HashSet<Animal>();
            this.CrossBreeds = new HashSet<CrossBreed>();
        }
    
        public int Animal_Breed_ID { get; set; }
        public string Animal_Breed_Name { get; set; }
        public string Animal_Breed_Description { get; set; }
        public Nullable<int> Animal_Type_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual Animal_Type Animal_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CrossBreed> CrossBreeds { get; set; }
    }
}
