﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Wollies_ShelterEntities : DbContext
    {
        public Wollies_ShelterEntities()
            : base("name=Wollies_ShelterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Adopter> Adopters { get; set; }
        public virtual DbSet<Adopter_Contract> Adopter_Contract { get; set; }
        public virtual DbSet<Adopter_Relative> Adopter_Relative { get; set; }
        public virtual DbSet<Adopter_Status> Adopter_Status { get; set; }
        public virtual DbSet<Adoption> Adoptions { get; set; }
        public virtual DbSet<Adoption_Status> Adoption_Status { get; set; }
        public virtual DbSet<AdoptionPayment> AdoptionPayments { get; set; }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Animal_Breed> Animal_Breed { get; set; }
        public virtual DbSet<Animal_Kennel_History> Animal_Kennel_History { get; set; }
        public virtual DbSet<Animal_Status> Animal_Status { get; set; }
        public virtual DbSet<Animal_Treatment> Animal_Treatment { get; set; }
        public virtual DbSet<Animal_Type> Animal_Type { get; set; }
        public virtual DbSet<Audit_Log> Audit_Log { get; set; }
        public virtual DbSet<CrossBreed> CrossBreeds { get; set; }
        public virtual DbSet<Customer_Event> Customer_Event { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<Donation_Line> Donation_Line { get; set; }
        public virtual DbSet<Donation_Type> Donation_Type { get; set; }
        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<Emp_Kennel> Emp_Kennel { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Employee_Status> Employee_Status { get; set; }
        public virtual DbSet<Employee_Type> Employee_Type { get; set; }
        public virtual DbSet<Event_> Event_ { get; set; }
        public virtual DbSet<Event_Schedule> Event_Schedule { get; set; }
        public virtual DbSet<Event_Type> Event_Type { get; set; }
        public virtual DbSet<Foster_Care> Foster_Care { get; set; }
        public virtual DbSet<Foster_Care_Parent> Foster_Care_Parent { get; set; }
        public virtual DbSet<GoogleChartData> GoogleChartDatas { get; set; }
        public virtual DbSet<HomeCheck> HomeChecks { get; set; }
        public virtual DbSet<HomeCheckReport> HomeCheckReports { get; set; }
        public virtual DbSet<Kennel> Kennels { get; set; }
        public virtual DbSet<Mecidal_Card> Mecidal_Card { get; set; }
        public virtual DbSet<Microchip> Microchips { get; set; }
        public virtual DbSet<Packaging_Type> Packaging_Type { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Payment_Type> Payment_Type { get; set; }
        public virtual DbSet<Role_> Role_ { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Stock_Type> Stock_Type { get; set; }
        public virtual DbSet<TimeSheet> TimeSheets { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<Unit_Type> Unit_Type { get; set; }
        public virtual DbSet<User_> User_ { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Vet_Appointment_Master> Vet_Appointment_Master { get; set; }
        public virtual DbSet<VetAppReason> VetAppReasons { get; set; }
        public virtual DbSet<Veterinarian> Veterinarians { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<Volunteer_Hours> Volunteer_Hours { get; set; }
        public virtual DbSet<Volunteer_Work_Type> Volunteer_Work_Type { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
        public virtual DbSet<tblFileDetail> tblFileDetails { get; set; }
        public virtual DbSet<tblFileDetail1> tblFileDetail1 { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<Wolly> Wollies { get; set; }
        public virtual DbSet<MedCardFile> MedCardFiles { get; set; }
        public virtual DbSet<AnimalTypePrice> AnimalTypePrices { get; set; }
    
        public virtual ObjectResult<AnimalType_SearchAnimalType_Result> AnimalType_SearchAnimalType(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AnimalType_SearchAnimalType_Result>("AnimalType_SearchAnimalType", nameParameter);
        }
    
        public virtual ObjectResult<BreedType_SearchBreedType_Result> BreedType_SearchBreedType(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BreedType_SearchBreedType_Result>("BreedType_SearchBreedType", nameParameter);
        }
    
        public virtual ObjectResult<Don_SearchDon_Result> Don_SearchDon(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Don_SearchDon_Result>("Don_SearchDon", nameParameter);
        }
    
        public virtual ObjectResult<Donor_SearchDonor_Result> Donor_SearchDonor(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Donor_SearchDonor_Result>("Donor_SearchDonor", nameParameter);
        }
    
        public virtual ObjectResult<Emp_SearchEmpType_Result> Emp_SearchEmpType(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Emp_SearchEmpType_Result>("Emp_SearchEmpType", nameParameter);
        }
    
        public virtual ObjectResult<Parent_SearchParent_Result> Parent_SearchParent(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Parent_SearchParent_Result>("Parent_SearchParent", nameParameter);
        }
    
        public virtual ObjectResult<Stock_SearchStock_Result> Stock_SearchStock(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Stock_SearchStock_Result>("Stock_SearchStock", nameParameter);
        }
    
        public virtual ObjectResult<Stock_SearchStockType_Result> Stock_SearchStockType(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Stock_SearchStockType_Result>("Stock_SearchStockType", nameParameter);
        }
    
        public virtual ObjectResult<Vet_SearchVet_Result> Vet_SearchVet(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Vet_SearchVet_Result>("Vet_SearchVet", nameParameter);
        }
    }
}
