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
    
    public partial class Adopter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Adopter()
        {
            this.Adopter_Contract = new HashSet<Adopter_Contract>();
            this.Adoptions = new HashSet<Adoption>();
            this.Adopter_Relative = new HashSet<Adopter_Relative>();
            this.Foster_Care_Parent = new HashSet<Foster_Care_Parent>();
        }


        public Adopter adopter { get; set; }
        public Adoption adoption { get; set; }

        [Display(Name = "Adopter")]
        [Required]
        public int Adopter_ID { get; set; }

        [Display(Name = "Adopter Name")]
        [Required]
        public string Adopter_Name { get; set; }

        [Display(Name = "Adopter Surname")]
        [Required]
        public string Adopter_Surname { get; set; }

        [Display(Name = "Adopter Email")]
        [Required]
        [EmailAddress]
        public string Adopter_Email { get; set; }

        [Display(Name = "Adopter Title")]
        [Required]
        public Nullable<int> Title_ID { get; set; }
        [Display(Name = "Adopter Address")]
        [Required]
        public string Adopter_Address { get; set; }


        [Display(Name = "Adopter Postal Address")]
        [Required]
        public string Adopter_PostalAddress { get; set; }


        [Display(Name = "Adopter Home Number")]
        [Required]
        public string Adopter_HomeNumber { get; set; }


        [Display(Name = "Adopter Work Number")]
        [Required]
        public string Adopter_WorkNumber { get; set; }


        [Display(Name = "Adopter Cell Number")]
        [Required]
        public string Adopter_CellNumber { get; set; }

        [Display(Name = "Adopter Car Registration Number")]
        [Required]
        public string Adopter_CarRegistartion_Number { get; set; }


        [Display(Name = "Adopter Employer")]
        [Required]
        public string Adopter_Employer { get; set; }


        [Display(Name = "Adopter Status")]
        public Nullable<int> Adopter_Status_ID { get; set; }


        [Display(Name = "Amount of Family Members")]
        [Required]
        public Nullable<int> Amount_of_Family_Memebers { get; set; }


        [Display(Name = "Number of Children")]
        [Required]
        public Nullable<int> No_of_Children { get; set; }


        [Display(Name = "How Many Children Under the Age of 12")]
        [Required]
        public Nullable<int> Age_of_Children { get; set; }


        [Display(Name = "Is the Property Securely Closed?")]
        [Required]
        public Nullable<bool> Property_Securely_Closed { get; set; }


        [Display(Name = "Does the Property Include a Pool?")]
        [Required]
        public Nullable<bool> Properyty_Include_Pool { get; set; }


        [Display(Name = "Is the Pool Secured?")]
        [Required]
        public Nullable<bool> Pool_Secured { get; set; }


        [Display(Name = "Is There an Animal Shelter Available?")]
        [Required]
        public Nullable<bool> Animal_Shelter_Available { get; set; }


        [Display(Name = "Does the Adopter have a Sick Animal?")]
        [Required]
        public Nullable<bool> Sick_Animal { get; set; }


        [Display(Name = "Sick Animal Diagnosis")]
        public string Sick_Animal_Diagnosis { get; set; }


        [Display(Name = "Animal's Sleep Location")]
        [Required]
        public string Animal_Sleep_Location { get; set; }


        [Display(Name = "Has the Adopter Given an Animal Away?")]
        [Required]
        public Nullable<bool> Given_Animal_Away { get; set; }


        [Display(Name = "Homecheck Suburb Location")]
        [Required]
        public string HomeCheck_Suburb { get; set; }


        [Display(Name = "Type of House")]
        [Required]
        public string Type_of_House { get; set; }


        [Display(Name = "Has the Adopter Adopted Before?")]
        [Required]
        public Nullable<bool> Adopted_Before { get; set; }


        [Display(Name = "Is the House is Enclosed in an Estate/Complex?")]
        [Required]
        public Nullable<bool> Complex_or_Flat { get; set; }


        [Display(Name = "Are Animals Allowed?")]
        [Required]
        public Nullable<bool> Animal_Allowed { get; set; }


        [Display(Name = "Animal Captivity: Caged or Free")]
        [Required]
        public string Animal_Captivity { get; set; }


        [Display(Name = "Are the Current Animal's Vaccines Updated?")]
        [Required]
        public Nullable<bool> Animal_Vaccines_Updated { get; set; }


        [Display(Name = "Adopter Occupation")]
        [Required]
        public string Adopter_Occupation { get; set; }


        [Display(Name = "Title")]
        public virtual Title Title { get; set; }
        
        public virtual Adopter_Status Adopter_Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adopter_Contract> Adopter_Contract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adoption> Adoptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adopter_Relative> Adopter_Relative { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foster_Care_Parent> Foster_Care_Parent { get; set; }
    }
}
