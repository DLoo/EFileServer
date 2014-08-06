using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using Ames.Entities;
using Ames.Abstract;
using Ames.Domain;
using System.Threading.Tasks;

namespace EFileServer.Controllers
{
    public class EFileServerController : ApiController
    {
        string AppRootPath = ConfigurationManager.AppSettings["AppRootPath"] ?? @"C:\EFileServerData";
        I_EFileRepository resp = new EFileRepository();

        [HttpGet]
        public string GetEFile() {
            return "Hello me!";
        }

        [HttpPost]
        public EFileInfo PostEFile(int year, int month, string location, string brand, string department, string type,
            string generateFrom, int expiryDuration) 
        {
            EFileInfo eFile = null;

            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

           // HttpResponseMessage result = null;
            HttpPostedFileBase media = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 1) {
                media = new HttpPostedFileWrapper(httpRequest.Files[0]);
            }
         
            MemoryStream mStream = new MemoryStream();
            media.InputStream.CopyTo(mStream);
            UploadFileInfo fMedia = new UploadFileInfo {
                Name = "media",
                FileName = media.FileName,
                ByteArray = mStream.ToArray(),
            };
            try {
                eFile = resp.Upload_File(AppRootPath, year, month, location, brand, department, type, generateFrom, expiryDuration, fMedia);
            } catch (InvalidOperationException ex) {
                throw new NotImplementedException(ex.Message, new Exception(ex.InnerException.ToString()));
            }
            return eFile;

        }

    }
}
