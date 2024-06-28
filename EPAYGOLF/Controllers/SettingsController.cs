using Entity;
using Microsoft.AspNetCore.Mvc;
using Repository.SalesStore;
using Repository.Settings;
using System.Globalization;

namespace EPAYGOLF.Controllers
{
	[ConditionalAuthorize(requireAuthorization: true)]
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
			try
			{
				if (obj != null)
				{
					if (!string.IsNullOrEmpty(obj.strYearStartDate))
					{
						var currentYear = DateTime.Now.Year;
						var date = obj.strYearStartDate + "/" + currentYear;
						obj.YearStartDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
					}
					if (obj.liabilitypct > 0)
					{
						obj.liabilitypct = obj.liabilitypct / 100;
					}
				}
				var result = _settingsRepository.SaveSettingsInformation(obj);
				return Json(result);

			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return Json(-2);
			}
		}
	}
}
