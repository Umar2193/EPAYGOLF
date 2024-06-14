using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DALSettings
	{
		DapperManager dapper;
		public DALSettings()
		{
			dapper = new DapperManager();
		}
		public List<SettingsEntity> GetSettingsList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [SettingID] " +
			$", [YearStartDate] " +
			$",[MonthStartDate]" +
			$",[WeekStartDay]" +
			$",isnull([liabilitypct],0.0) * 100 as liabilitypct" +
			$", [IsActive]	" +
			$", [IsDeleted]	" +
			$", [CreatedAt]	" +
			$", [CreatedBy]	" +
			$", [UpdatedAt]	" +
			$", [UpdatedBy]	" +
	  $"  FROM[dbo].[Settings] " +
	  $"  where IsActive = 1 and IsDeleted = 0 " +
	  $"  order by CreatedAt desc");
			var Data = dapper.Query<SettingsEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public SettingsEntity GetSettingsDetail(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [SettingID] " +
			$", [YearStartDate] " +
			$",[MonthStartDate]" +
			$",[WeekStartDay]" +
			$",isnull([liabilitypct],0.0) * 100 as liabilitypct" +
			$", [IsActive]	" +
			$", [IsDeleted]	" +
			$", [CreatedAt]	" +
			$", [CreatedBy]	" +
			$", [UpdatedAt]	" +
			$", [UpdatedBy]	" +
	  $"  FROM[dbo].[Settings] " +
	  $"  where IsActive = 1 and IsDeleted = 0 " +
			$" and SettingID = " + id);
			var Data = dapper.Query<SettingsEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveSettingsInformation(SettingsEntity obj)
		{
			string Query = "";
			if (obj.SettingID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Settings] " +
					$"SET [YearStartDate] = '" + obj.YearStartDate + "'" +
					$",[MonthStartDate] = '" + obj.MonthStartDate + "' " +
					$",[WeekStartDay] = '" + obj.WeekStartDay + "' " +
					$",[liabilitypct] = '" + obj.liabilitypct + "' " +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [SettingID] = '" + obj.SettingID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Settings]" +
					$" ([YearStartDate] ,[MonthStartDate] ,[WeekStartDay] ,[liabilitypct]" +
					$",[IsActive],[IsDeleted],[UpdatedAt],[CreatedAt],[CreatedBy],[UpdatedBy] )" +
					$" VALUES('" + obj.YearStartDate + "','" + obj.MonthStartDate + "'" +
					$",'" + obj.WeekStartDay + "', '" + obj.liabilitypct + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteSettingsInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Settings] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [SettingID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
	}
}
