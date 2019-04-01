using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DoctorRatingsServer.Controllers
{
	[Route("{*url}")]
	public class NotFound : Controller
	{
		// Unknown 404 
		[HttpGet]
		[HttpPost]
		public object GetUnknown()
		{
			object jsonResponse = new
			{
				service_error = new
				{
					success = false,
					error = 404,
					message = "Invalid URL Request"
				}
			};
			return jsonResponse;
		}
	}

	[Route("api/home")]
	public class HomeController : Controller
	{
		// GET api/home
		[Route("")]
		[HttpGet]
		public ContentResult Get()
		{
			return new ContentResult
			{
				ContentType = "text/html",
				StatusCode = (int)HttpStatusCode.OK,
				Content =
					"<html><body style='" +
						"color: #03A9F4; background-color: #1d1d1d; margin: 30px; font-family: \"Segoe UI\", Arial, sans-serif;'>" +
					"<h1>Doctor Ratings Server</h1>" +
					"<div>Version 0.1.0</div>" +
					"<div style='" +
						"background-color: green; margin: 30px 0px; color: white; padding: 14px; width: 100; text-align: center;'>" +
					"<span>RUNNING!</span></div>" +
					"</body></html>"
			};
		}
	}
}
