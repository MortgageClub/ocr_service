using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Upload_File_To_ASPNET_Web_API_Client.Infrastructure;
using Upload_File_To_ASPNET_Web_API_Models;

namespace Upload_File_To_ASPNET_Web_API_Client
{
	internal class Program
	{
		private static readonly string DownloadFolder = "downloads";

		private static void Main(string[] args)
		{
			UploadFiles();
		}

		private static async void UploadFiles()
		{
			Uri server = new Uri("http://localhost:4260/api/upload");
			HttpClient httpClient = new HttpClient();

			StringContent stringContent = new StringContent("Broken Sword: The Shadow of the Templars (also known as Circle of Blood in the United States)[1] is a 1996 point-and-click adventure game developed by Revolution Software. The player assumes the role of George Stobbart, an American tourist in Paris, as he attempts to unravel a conspiracy. The game takes place in both real and fictional locations in Europe and the Middle East.", Encoding.UTF8, "text/plain");
			StreamContent streamConent = new StreamContent(new FileStream(@"..\..\TestData\HintDesk.png", FileMode.Open, FileAccess.Read, FileShare.Read));

			MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
			multipartFormDataContent.Add(stringContent, "Broken Sword", "Broken Sword.txt");
			multipartFormDataContent.Add(streamConent, "HintDesk", "HintDesk.png");

			//HttpResponseMessage responseMessage = await httpClient.PostAsync(server, multipartFormDataContent);
			HttpResponseMessage responseMessage = httpClient.PostAsync(server, multipartFormDataContent).Result;

			if (responseMessage.IsSuccessStatusCode)
			{
				IList<HDFile> hdFiles = await responseMessage.Content.ReadAsAsync<IList<HDFile>>();
				if (Directory.Exists(DownloadFolder))
					(new DirectoryInfo(DownloadFolder)).Empty();
				else
					Directory.CreateDirectory(DownloadFolder);

				foreach (HDFile hdFile in hdFiles)
				{
					responseMessage = httpClient.GetAsync(new Uri(hdFile.Url)).Result;

					if (responseMessage.IsSuccessStatusCode)
					{
						using (FileStream fs = File.Create(Path.Combine(DownloadFolder, hdFile.Name)))
						{
							Stream streamFromService = await responseMessage.Content.ReadAsStreamAsync();
							streamFromService.CopyTo(fs);
						}
					}
				}
			}
		}
	}
}