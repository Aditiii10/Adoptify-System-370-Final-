using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdoptifySystem;
using System.Data.SqlClient;
using System.IO;

namespace AdoptifySystem.Controllers
{
    public class BackupController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();
        // GET: Backup
        public ActionResult BackupDatabase()
        {
            string dbPath = Server.MapPath("~/App_Data/DBBackup.bak");
            using (var db = new Wollies_ShelterEntities())
            {
                var cmd = String.Format("BACKUP DATABASE {0} TO DISK='{1}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for {0} database';", "Wollies_Shelter", dbPath);
                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);
            }
            return new FilePathResult(dbPath, "application/octet-stream");
        }

        public ActionResult RestoreDatabase()
        {
            string dbPath = Server.MapPath("~/App_Data/DBBackup.bak");
            using (var db = new Wollies_ShelterEntities())
            {

                var cmd = String.Format("USE master restore DATABASE Wollies_Shelter from DISK='{0}' WITH REPLACE;", dbPath);
                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);
            }
            return View();
        }
    }
}