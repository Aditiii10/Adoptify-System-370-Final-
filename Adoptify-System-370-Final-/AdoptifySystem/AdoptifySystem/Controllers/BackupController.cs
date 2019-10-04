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
        // GET: Backup
        public ActionResult Index()
        {
            return View();
        }




        private SqlConnection conn;
        string connectionString = "Data Source=wollies.database.windows.net;Initial Catalog =Wollies_Shelter; integrated security=true; persist security info=True;user id=user;password=Wollies123;Trusted_Connection=false;Encrypt=True";



        public ActionResult Backup()
        {
            ViewBag.Message = "Click to backup database";

            return View();
        }

        [HttpPost]
        public ActionResult Backup(string message)
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string str = "USE Wollies_Shelter;";
                string str1 = "BACKUP DATABASE Wollies_Shelter TO DISK = 'C:\\Users\\Divin\\Desktop\\Wollies_Shelter.Bak' WITH FORMAT,MEDIANAME = 'Z_SQLServerBackups',NAME = 'Full Backup of Wollies_Shelter';";
                SqlCommand cmd1 = new SqlCommand(str, conn);
                SqlCommand cmd2 = new SqlCommand(str1, conn);
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                conn.Close();
                //conn.Dispose();
                message = "Successful Backup";

            }
            catch (Exception ex)
            {
                message = Convert.ToString(ex);
            }

            ViewBag.Message = message;

            return View();
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult BackupDatabase()
        //{

        //    string dbPath = Server.MapPath("~/App_Data/DBBackup.bak");
        //   SqlConnection db = new SqlConnection(@"Data Source=wollies.database.windows.net;Initial Catalog =Wollies_Shelter; integrated security=true; persist security info=True;user id=user;password=Wollies123;Trusted_Connection=false;Encrypt=True");

        //    {
        //        var cmd = String.Format("BACKUP DATABASE {0} TO DISK='{1}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for {0} database';", "Wollies_Shelter", dbPath);
        //        db.Database.ExecuteNonQuery(TransactionalBehavior.DoNotEnsureTransaction, cmd);
        //    }
        //    return new FilePathResult(dbPath, "application/octet-stream");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RestoreDatabase()
        //{
        //    string dbPath = Server.MapPath("~/App_Data/DBBackup.bak");
        //    using (var db = new Wollies_ShelterEntities())
        //    {

        //        var cmd = String.Format("USE master restore DATABASE Wollies_Shelter from DISK='{0}' WITH REPLACE;", dbPath);
        //       db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);
        //    }
        //    return View();
        //}

    }
}