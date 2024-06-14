using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Settings
{
	public interface ISettingsRepository
	{
		List<SettingsEntity> GetSettingsList();
		SettingsEntity GetSettingsDetail(Int64 ID);
		int SaveSettingsInformation(SettingsEntity obj);
		int DeleteSettingsInformation(Int64 ID);
	}
}
