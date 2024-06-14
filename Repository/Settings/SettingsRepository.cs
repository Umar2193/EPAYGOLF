using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Settings
{
	public class SettingsRepository:ISettingsRepository
	{
		DAL.DALSettings _DALSettings = new DAL.DALSettings();
		public List<SettingsEntity> GetSettingsList()
		{
			try
			{

				return _DALSettings.GetSettingsList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public SettingsEntity GetSettingsDetail(Int64 ID)
		{
			try
			{

				return _DALSettings.GetSettingsDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveSettingsInformation(SettingsEntity obj)
		{
			try
			{

				return _DALSettings.SaveSettingsInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteSettingsInformation(Int64 ID)
		{
			try
			{

				return _DALSettings.DeleteSettingsInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
	}
}
