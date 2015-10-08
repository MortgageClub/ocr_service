using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Upload_File_To_ASPNET_Web_API.Controllers
{
	public class DirectUploadController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}