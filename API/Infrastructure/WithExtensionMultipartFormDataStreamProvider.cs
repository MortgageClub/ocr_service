using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using Upload_File_To_ASPNET_Web_API.Infrastructure.Util;

namespace Upload_File_To_ASPNET_Web_API.Infrastructure
{
	public class WithExtensionMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
	{
		public WithExtensionMultipartFormDataStreamProvider(string rootPath)
			: base(rootPath)
		{
		}

		public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
		{
			string extension = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? Path.GetExtension(OSUtil.GetValidFileName(headers.ContentDisposition.FileName)) : "";
			return Guid.NewGuid().ToString() + extension;
		}
	}
}