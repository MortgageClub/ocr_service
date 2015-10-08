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
using Upload_File_To_ASPNET_Web_API.Infrastructure;
using Upload_File_To_ASPNET_Web_API_Models;

namespace Upload_File_To_ASPNET_Web_API.Controllers
{
	public class UploadController : ApiController
	{
		private ILog log = log4net.LogManager.GetLogger(typeof(UploadController));
		private const string UploadFolder = "import";

		public HttpResponseMessage Get(string fileName)
		{
			HttpResponseMessage result = null;

			DirectoryInfo directoryInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/" + UploadFolder));
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

		public Task<IQueryable<HDFile>> Post()
		{
			try
			{
				var uploadFolderPath = HostingEnvironment.MapPath("~/App_Data/" + UploadFolder);
				log.Debug(uploadFolderPath);

				if (Request.Content.IsMimeMultipartContent())
				{
					var streamProvider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath);
					var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IQueryable<HDFile>>(t =>
					{
						if (t.IsFaulted || t.IsCanceled)
						{
							throw new HttpResponseException(HttpStatusCode.InternalServerError);
						}

						var fileInfo = streamProvider.FileData.Select(i =>
						{
							var info = new FileInfo(i.LocalFileName);
							return new HDFile(info.Name, Request.RequestUri.AbsoluteUri + "?filename=" + info.Name, (info.Length / 1024).ToString());
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