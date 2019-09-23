﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using AdoptifySystem.Models;
using AdoptifySystem;

namespace AdoptifySystem.Controllers
{
    public class MedCardFileController : Controller
    {
        string conString = "Data Source=wollies.database.windows.net;Initial Catalog =Wollies_Shelter; integrated security=true; persist security info=True;user id=user;password=Wollies123;Trusted_Connection=false;Encrypt=True";

        public ActionResult FileView(MedCardFileUpload model)
        {
            List<MedCardFileUpload> list = new List<MedCardFileUpload>();
            DataTable dtFiles = GetFileDetails();
            foreach (DataRow dr in dtFiles.Rows)
            {
                list.Add(new MedCardFileUpload
                {
                    FileId = @dr["SQLID"].ToString(),
                    FileName = @dr["FILENAME"].ToString(),
                    FileUrl = @dr["FILEURL"].ToString()
                });
            }
            model.FileList = list;
            return View(model);
        }
        [HttpPost]
        public ActionResult FileView(HttpPostedFileBase files)
        {
            MedCardFileUpload model = new MedCardFileUpload();
            List<MedCardFileUpload> list = new List<MedCardFileUpload>();
            DataTable dtFiles = GetFileDetails();
            foreach (DataRow dr in dtFiles.Rows)
            {
                list.Add(new MedCardFileUpload
                {
                    FileId = @dr["SQLID"].ToString(),
                    FileName = @dr["FILENAME"].ToString(),
                    FileUrl = @dr["FILEURL"].ToString()
                });
            }
            model.FileList = list;

            if (files != null)
            {
                var Extension = Path.GetExtension(files.FileName);
                var fileName = "my-file-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                string path = Path.Combine(Server.MapPath("~/UploadedFilesMedCard"), fileName);
                model.FileUrl = Url.Content(Path.Combine("~/UploadedFilesMedCard/", fileName));
                model.FileName = fileName;

                if (SaveFile(model))
                {
                    files.SaveAs(path);
                    TempData["AlertMessage"] = "Uploaded Successfully !!";
                    return RedirectToAction("FileView", "MedCardFile");
                }
                else
                {
                    ModelState.AddModelError("", "Error In Add File. Please Try Again !!!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Choose Correct File Type !!");
                return View(model);
            }
            return RedirectToAction("FileView", "MedCardFile");
        }
        private DataTable GetFileDetails()
        {
            DataTable dtData = new DataTable();
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand command = new SqlCommand("Select * From MedCardFile", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dtData);
            con.Close();
            return dtData;
        }

        private bool SaveFile(MedCardFileUpload model)
        {
            string strQry = "INSERT INTO MedCardFile (FileName,FileUrl) VALUES('" +
                model.FileName + "','" + model.FileUrl + "')";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand command = new SqlCommand(strQry, con);
            int numResult = command.ExecuteNonQuery();
            con.Close();
            if (numResult > 0)
                return true;
            else
                return false;
        }

        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}