using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Ames.Infrastructue;
using Ames.Domain;
using Ames.Entities;
using Ames.Abstract;

namespace EFileServer.Controllers {
    public class HomeController : Controller {
        string AppRootPath = ConfigurationManager.AppSettings["AppRootPath"] ?? @"C:\EFileServerData";
        I_EFileRepository resp = new EFileRepository();

        [ProfileAction]
        public ActionResult Index() {
            return View();
        }

        [HandleError(ExceptionType = typeof(FileNotFoundException), View = "ErrorFileNotFound")]
        [ProfileAction]
        public ActionResult GetEFile(string fileGuid) {

            if (String.IsNullOrEmpty(fileGuid))
                return RedirectToAction("Index");
            
            EFileInfo eFile = resp.Get_EFileByGUID(fileGuid);
            var downloadFileName = eFile.DirectoryPath + eFile.EFileName;

            if (System.IO.File.Exists(downloadFileName))
            {
                return File(downloadFileName, MediaTypeNames.Application.Octet, eFile.EFileName);
            }
            else
                throw new FileNotFoundException("File Not Found in our server.", eFile.EFileName);         
        }

        [HttpGet]
        public ActionResult UploadEFile() 
        {
            return View();
        }


        [HttpPost]
        [ProfileAction]
        public ActionResult UploadEFile(int year, int month, string location, string brand, string department, string type, 
            string generateFrom, int expiryDuration, HttpPostedFileBase media) {

            EFileInfo eFile = null;
            
            
            try {
                MemoryStream mStream = new MemoryStream();
                
                media.InputStream.CopyTo(mStream);
                UploadFileInfo fMedia = new UploadFileInfo{
                    Name = "media",
                    FileName = media.FileName,
                    ByteArray = mStream.ToArray(),
                };
                eFile = resp.Upload_File(AppRootPath, year, month, location, brand, department, type, generateFrom, expiryDuration, fMedia);
            } catch (InvalidOperationException ex) {
                ModelState.AddModelError(ex.Message, ex.InnerException.ToString());
            }
            

            return View("Index", eFile);
        }

 
    }
}