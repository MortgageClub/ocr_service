using System.Web;
using System.Web.Mvc;

namespace Upload_File_To_ASPNET_Web_API
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}