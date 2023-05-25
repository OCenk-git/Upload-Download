using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Upload_Download.Models;

namespace Upload_Download.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UploadFile(IFormFile dieDatei)
		{
			NETDBContext context = new NETDBContext();
			DocStore docStore = new DocStore();

			if (dieDatei == null || dieDatei.Length == 0)
			{
				return Content("Keine Datei ausgewählt oder die Datei ist leer");
			}

			using(MemoryStream stream = new MemoryStream()) 
			{
				dieDatei.CopyTo(stream);
				docStore.DocData = stream.ToArray();
			}
			docStore.ContentType = dieDatei.ContentType;
			docStore.ContentLength = dieDatei.Length;
			docStore.InsertionDate = DateTime.Now;
			docStore.DocName = dieDatei.FileName;

			context.Add(docStore);
			context.SaveChanges();

			return RedirectToAction("ShowFiles");
		}

		public IActionResult ShowFiles()
		{
			NETDBContext ctx = new NETDBContext();

			return View(ctx);
		}

		public IActionResult DateiDownload(string filename)
		{
			NETDBContext context = new NETDBContext();
			DocStore docStore = new DocStore();

			if (filename == null)
			{
				return Content("Kein Dateiname angegeben.");
			}

			foreach(var item in context.DocStores)
			{
				if(item.DocName == filename)
				{
					docStore.DocName = filename;
					docStore.ContentType = item.ContentType;
					docStore.DocData = item.DocData;
				}
			}

			return File(docStore.DocData,docStore.ContentType,docStore.DocName);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}