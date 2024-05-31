using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DALVATRates
	{
		DapperManager dapper;
		public DALVATRates()
		{
			dapper = new DapperManager();
		}
		public List<VATRatesEntity> GetVATRatesList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [VATRatesID] " +
			$", [VATRateDate] " +
			$", [VATRate]		" +
			$", [IsActive]	" +
			$", [IsDeleted]	" +
			$", [CreatedAt]	" +
			$", [CreatedBy]	" +
			$", [UpdatedAt]	" +
			$", [UpdatedBy]	" +
	  $"  FROM[dbo].[VatRates] " +
	  $"  where IsActive = 1 and IsDeleted = 0 " +
	  $"  order by VATRateDate desc");
			var Data = dapper.Query<VATRatesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public VATRatesEntity GetVATRatesDetail(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [VATRatesID] " +
			$", [VATRateDate] " +
			$", [VATRate]		" +
			$", [IsActive]	" +
			$", [IsDeleted]	" +
			$", [CreatedAt]	" +
			$", [CreatedBy]	" +
			$", [UpdatedAt]	" +
			$", [UpdatedBy]	" +
	        $"  FROM[dbo].[VatRates] " +
	        $"  where IsActive = 1 and IsDeleted = 0 " +
		    $" and VATRatesID = " + id);
			var Data = dapper.Query<VATRatesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveVATRatesInformation(VATRatesEntity obj)
		{
			string Query = "";
			if (obj.VATRatesID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[VatRates] SET [VATRateDate] = '" + obj.VATRateDate + "'" +
					$",[VATRate] = '" + obj.VATRate + "' " +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [VATRatesID] = '" + obj.VATRatesID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[VatRates] ([VATRateDate] ,[VATRate],[IsActive],[IsDeleted],[UpdatedAt],[CreatedAt],[CreatedBy],[UpdatedBy] )" +
					$" VALUES('" + obj.VATRateDate + "','" + obj.VATRate + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteVATRatesInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[VatRates] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [VATRatesID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
	}
}
