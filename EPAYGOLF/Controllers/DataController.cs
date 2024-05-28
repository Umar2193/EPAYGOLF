using Microsoft.AspNetCore.Mvc;

namespace EPAYGOLF.Controllers
{
	public class DataController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
