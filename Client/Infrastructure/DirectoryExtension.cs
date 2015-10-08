using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure
{
	public static class DirectoryExtension
	{
		public static void Empty(this DirectoryInfo directory)
		{
			if (directory.Exists)
			{
				foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
				foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
			}
		}
	}
}