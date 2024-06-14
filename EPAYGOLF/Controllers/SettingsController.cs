using Entity;
using Microsoft.AspNetCore.Mvc;
using Repository.SalesStore;
using Repository.Settings;

namespace EPAYGOLF.Controllers
{
	public class SettingsController : Controller
	{
		private ISettingsRepository _settingsRepository = new SettingsRepository();
		public IActionResult Index()
		{
			var model = _settingsRepository.GetSettingsList().FirstOrDefault() ;
			if (model == null)
			{
				model = new SettingsEntity();
			}
			return View(model);
			
		}
		
		[HttpPost]
		public IActionResult SaveSettings(SettingsEntity obj)
		{
			if(obj != null)
			{
				if (obj.liabilitypct > 0)
				{
					obj.liabilitypct = obj.liabilitypct / 100;
				}
			}
			var result = _settingsRepository.SaveSettingsInformation(obj);
			return Json(result);
		}
	}
}
