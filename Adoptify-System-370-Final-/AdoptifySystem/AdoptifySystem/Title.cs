namespace AdoptifySystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;  //Display annotation
    using System.ComponentModel;  //DisplayName annotation

    public partial class Title
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Title()
        {
            this.Adopters = new HashSet<Adopter>();
            this.Donors = new HashSet<Donor>();
            this.Employees = new HashSet<Employee>();
            this.Volunteers = new HashSet<Volunteer>();
        }

        [Display(Name = "Title")]
        public int Title_ID { get; set; }

        [Display(Name = "Title Name")]
        public string Title_Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adopter> Adopters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donor> Donors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}