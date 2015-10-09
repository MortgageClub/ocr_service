using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using API.Infrastructure;
using Models;
using System.ComponentModel;

namespace API.Controllers
{
	public class UploadController : ApiController
	{
		private ILog log = log4net.LogManager.GetLogger(typeof(UploadController));
		private const string ImportFolder = "C:/_FlexiCapture_Import";
        private const string ExportFolder = "C:/_FlexiCapture_Export";

        public HttpResponseMessage Get(string fileName)
		{
			HttpResponseMessage result = null;

			DirectoryInfo directoryInfo = new DirectoryInfo(ImportFolder);
			FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == fileName).FirstOrDefault();
			if (foundFileInfo != null)
			{
				FileStream fs = new FileStream(foundFileInfo.FullName, FileMode.Open);

				result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new StreamContent(fs);
				result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
				result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
				result.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;
			}
			else
			{
				result = new HttpResponseMessage(HttpStatusCode.NotFound);
			}

			return result;
		}

        public HttpResponseMessage Post(string url)
        {
            HttpResponseMessage result = null;

            // Create an instance of WebClient
            WebClient client = new WebClient();
            // Hookup DownloadFileCompleted Event
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

            // Start the download and copy the file to 
            DirectoryInfo directoryInfo = new DirectoryInfo(ImportFolder);
            client.DownloadFileAsync(new Uri(url), directoryInfo + "/test.jpg");

            
            FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == url).FirstOrDefault();
            if (foundFileInfo != null)
            {
                FileStream fs = new FileStream(foundFileInfo.FullName, FileMode.Open);

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(fs);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return result;
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            log.Debug("File downloaded");
        }

        public Task<IQueryable<UploadFile>> Post()
		{
			try
			{
				log.Debug(ImportFolder);

				if (Request.Content.IsMimeMultipartContent())
				{
					var streamProvider = new WithExtensionMultipartFormDataStreamProvider(ImportFolder);
					var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IQueryable<UploadFile>>(t =>
					{
						if (t.IsFaulted || t.IsCanceled)
						{
							throw new HttpResponseException(HttpStatusCode.InternalServerError);
						}

						var fileInfo = streamProvider.FileData.Select(i =>
						{
							var info = new FileInfo(i.LocalFileName);
							return new UploadFile(info.Name, Request.RequestUri.AbsoluteUri + "?filename=" + info.Name, (info.Length / 1024).ToString());
						});
						return fileInfo.AsQueryable();
					});

					return task;
				}
				else
				{
					throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}
	}
}