using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;

namespace API.Controllers
{
    public class ResultController : ApiController
    {
        private const string ExportFolder = "C:/_FlexiCapture_Export";

        public HttpResponseMessage Get(string fileName)
        {
            HttpResponseMessage result = null;
            result = new HttpResponseMessage(HttpStatusCode.OK);

            DirectoryInfo directoryInfo = new DirectoryInfo(ExportFolder);
            FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == fileName).FirstOrDefault();
            if (foundFileInfo != null)
            {
                FileStream fs = new FileStream(foundFileInfo.FullName, FileMode.Open);
                result.Content = new StreamContent(fs);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;
            }
            else
            {
                result.Content = new StringContent("{\"message\": \"file not found\"}");
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            }

            return result;
        }
    }
}