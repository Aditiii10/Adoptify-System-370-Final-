using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifySystem.Models.nickeymodel
{
    public class Flexible
    {
        Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        //these are the list of classes
        public List<Employee> employeelist { get; set; }
        public List<Packaging_Type> packaging_Types { get; set; }
        public List<Unit_Type> unit_Types { get; set; }
        public List<Title> Titles { get; set; }
        public List<Donor> DonorList { get; set; }
        public List<Stock> Stocklist { get; set; }
        public List<Stock_Type> Stock_Types { get; set; }
        public List<Donation_Line> adddonationlist { get; set; }
        public List<Foster_Care> Fostercarelist { get; set; }
        public List<Animal> animallist { get; set; }
        public List<Foster_Care_Parent> fostercareparent { get; set; }
        public List<Subsystem> subsystemslist { get; set; }

        //these are the single classes
        public Donor donor { get; set; }
        public Employee employee { get; set; }
        public Foster_Care_Parent parent { get; set; }
        public Animal animal { get; set; }
        public Stock stock { get; set; }
        public Donation donation { get; set; }
        public User_ currentuser { get; set; }
        public Role_ role { get; set; }

        public bool CreateAuditTrail(int id, string usecase)
        {
            try
            {
                User_ user = db.User_.Where(z => z.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    Audit_Log audit = new Audit_Log();
                    audit.Transaction_Type = "Create";
                    audit.Auditlog_DateTime = DateTime.Now;
                    audit.UserID = user.UserID;
                    audit.Critical_Date = "Created a New" + usecase;
                    db.Audit_Log.Add(audit);
                    db.SaveChanges();
                    return true;
                }


            }
            catch (Exception e)
            {

                return false;
            }

        }
        public bool UpdateAuditTrail(int id, string usecase)
        {
            try
            {
                User_ user = db.User_.Where(z => z.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    Audit_Log audit = new Audit_Log();
                    audit.Transaction_Type = "Update";
                    audit.Auditlog_DateTime = DateTime.Now;
                    audit.UserID = user.UserID;
                    audit.Critical_Date = "Updated " + usecase;

                    db.SaveChanges();
                    return true;
                }


            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool DeleteAuditTrail(int id, string usecase)
        {
            try
            {
                User_ user = db.User_.Where(z => z.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    Audit_Log audit = new Audit_Log();
                    audit.Transaction_Type = "Delete";
                    audit.Auditlog_DateTime = DateTime.Now;
                    audit.UserID = user.UserID;
                    audit.Critical_Date = "Deleted a " + usecase;

                    db.SaveChanges();
                    return true;
                }


            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool LoginAuditTrail(int id, string usecase)
        {
            try
            {
                User_ user = db.User_.Where(z => z.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    Audit_Log audit = new Audit_Log();
                    audit.Transaction_Type = "Read";
                    audit.Auditlog_DateTime = DateTime.Now;
                    audit.UserID = user.UserID;
                    audit.Critical_Date = "Read a New" + usecase;

                    db.SaveChanges();
                    return true;
                }


            }
            catch (Exception)
            {

                return false;
            }

        }
        public bool Authorize(int id, int insub)
        {
            bool isvalid = false;
            try
            {
                User_ user = db.User_.Where(z => z.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    foreach (var role in user.UserRoles)
                    {
                        foreach (var sub in role.Role_.SubsystemRoles)
                        {
                            if (sub.Subsystem_Id == insub)
                            {
                                isvalid = true;

                            }
                            break;
                        }
                        if (isvalid)
                        {
                            break;
                        }
                    }
                    return isvalid;
                }
            }
            catch (Exception)
            {

                isvalid = false;
            }
            return isvalid;
        }



    }
}