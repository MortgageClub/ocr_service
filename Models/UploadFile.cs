using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class UploadFile
	{
		public UploadFile(string name, string url, string size)
		{
			Name = name;
			Url = url;
			Size = size;
		}

		public string Name { get; set; }

		public string Url { get; set; }

		public string Size { get; set; }
	}
}