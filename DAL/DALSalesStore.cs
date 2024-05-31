using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DALSalesStore
	{
		DapperManager dapper;
		public DALSalesStore()
		{
			dapper = new DapperManager();
		}
		public List<SalesStoreEntity> GetSalesStoreList(Int64 ID = 0)
		{

			string Query = string.Format($"SELECT [CommisionID],[RetailerID],[RetailerName],[StoreName] " +
	   $"  , isnull([Commission], 0.00) * 100 as Commission " +
	   $" , isnull([StripeFee], 0.00) * 100 as StripeFee " +
	   $" , isnull([ProcessingFee], 0.00) * 100 as ProcessingFee " +
	   $" , isnull([GGCRedemption], 0.00) * 100 as GGCRedemption " +
	   $"  , isnull([TransactionFee], 0.00) * 100 as TransactionFee " +
	   $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
       $" FROM[EPAYGOLF].[dbo].[Commissions] " +
       $" Where IsActive = 1 and IsDeleted = 0 " +
       $" order by CreatedAt desc");
			var Data = dapper.Query<SalesStoreEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public SalesStoreEntity GetSalesStoreDetail(Int64 ID = 0)
		{

			string Query = string.Format($"SELECT [CommisionID],[RetailerID],[RetailerName],[StoreName] " +
	   $"  , isnull([Commission], 0.00) * 100 as Commission " +
	   $" , isnull([StripeFee], 0.00) * 100 as StripeFee " +
	   $" , isnull([ProcessingFee], 0.00) * 100 as ProcessingFee " +
	   $" , isnull([GGCRedemption], 0.00) * 100 as GGCRedemption " +
	   $"  , isnull([TransactionFee], 0.00) * 100 as TransactionFee " +
	   $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
	   $" FROM[EPAYGOLF].[dbo].[Commissions] " +
	   $" Where IsActive = 1 and IsDeleted = 0 " +
				$" and CommisionID = " + ID);
			var Data = dapper.Query<SalesStoreEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveSalesStoreInformation(SalesStoreEntity obj)
		{
			string Query = "";
			if (obj.CommisionID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Commissions] " +
					$"SET [RetailerID] = '" + obj.RetailerID + "'" +
					$",[RetailerName] = '" + obj.RetailerName + "' " +
					$",[StoreName] = '" + obj.StoreName + "'" +
					$",[Commission] ='" + obj.Commission + "'" +
					$",[StripeFee] ='" + obj.StripeFee + "'" +
					$",[ProcessingFee] ='" + obj.ProcessingFee + "'" +
					$",[GGCRedemption] ='" + obj.GGCRedemption + "'" +
					$",[TransactionFee] ='" + obj.TransactionFee + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [CommisionID] = '" + obj.CommisionID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Commissions] ([RetailerID],[RetailerName],[StoreName],[Commission],[StripeFee],[ProcessingFee],[GGCRedemption],[TransactionFee],[IsActive],[IsDeleted],[UpdatedAt],[CreatedAt],[CreatedBy],[UpdatedBy])" +
					$" VALUES('" + obj.RetailerID + "','" + obj.RetailerName + "'" +
					$",'" + obj.StoreName + "' ,'" + obj.Commission + "'" +
					$",'" + obj.StripeFee + "' ,'" + obj.ProcessingFee + "'" +
					$",'" + obj.GGCRedemption + "' ,'" + obj.TransactionFee + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteSalesStoreInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Commissions] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE CommisionID = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
	}
}

