using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DALRetailer
	{
		DapperManager dapper;
		public DALRetailer()
		{
			dapper = new DapperManager();
		}
		public List<RetailerEntity> GetRetailerList(Int64 ID=0)
		{

			string Query = string.Format($"SELECT r.[RetailerID]\r\n ,r.[RetailerName]\r\n " +
				$" ,r.[RetailerCode]\r\n   " +
				$",isnull(r.[Commission],0) * 100 as Commission\r\n " +
				$" FROM [Retailer] r \r\n" +
				$" where r.IsActive=1 and r.IsDeleted=0 " +
				$" order by RetailerName");
			var Data = dapper.Query<RetailerEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public RetailerEntity GetRetailerDetail(Int64 ID = 0)
		{

            string Query = string.Format($"SELECT r.[RetailerID]\r\n ,r.[RetailerName]\r\n " +
                $" ,r.[RetailerCode]\r\n   " +
                $",isnull(r.[Commission],0) * 100 as Commission\r\n " +
                $" FROM [Retailer] r \r\n" +
                $" where r.IsActive=1 and r.IsDeleted=0 " +
                $" order by RetailerName");
            var Data = dapper.Query<RetailerEntity>(Query, null, null, true, null, CommandType.Text);
            return Data.ToList().FirstOrDefault();
		}
		public int SaveRetailerInformation(RetailerEntity obj)
		{
			string Query = "";
			if (obj.RetailerID > 0 )
			{
				Query = string.Format($"UPDATE [dbo].[Retailer] SET " +
					$"[RetailerName] = '" + obj.RetailerName + "' " +
					$",[RetailerCode] = '" + obj.RetailerCode + "'" +
					$",[Commission] ='" + obj.Commission + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [RetailerID] = '" + obj.RetailerID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Retailer] ([RetailerName],[RetailerCode],[Commission] ,[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy] ,[UpdatedBy] )" +
					$" VALUES('" + obj.RetailerName + "'" +
					$",'" + obj.RetailerCode + "' ,'" + obj.Commission + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteRetailerInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Retailer] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [RetailerID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}

		#region BlackHawkSales
		public int SaveBlackHawkSalesInformation(BlackHawkSalesEntity obj)
		{
			string Query = "";
			
				Query = string.Format($"INSERT INTO [dbo].[BlackHawkSales] ([CompanyName],[MerchantName],[MerchantTransactionID],[SourceTransactionID],[ProductDescription],[CardID],[TransactionDateTime],[Value],[TransactionType],[StoreNo],[StoreName],[PAN],[EAN],[StoreCommission] ,[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy],[UpdatedBy] )" +
					$" VALUES('" + obj.CompanyName + "','" + obj.MerchantName + "'" +
					$",'" + obj.REFERENCENUMBER + "' ,'" + obj.SOURCETRANSACTIONID + "'" +
					$",'" + obj.PRODUCTDESCRIPTION + "' ,'" + obj.GIFTCARDNUMBER + "'" +
					$",'" + obj.TransactionDate + "' ,'" + obj.TRANSACTIONAMOUNT + "'" +
					$",'' ,'" + obj.StoreID + "'" +
					$",'" + obj.StoreName + "' ,'',''" +
					$",'0' " +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public List<BlackHawkSalesEntity> GetBlackHawkSalesList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT  [BlackHawkSalesID],[CompanyName],[MerchantName],[MerchantTransactionID] REFERENCENUMBER " +
	  $" ,[SourceTransactionID] SOURCETRANSACTIONID,[ProductDescription] PRODUCTDESCRIPTION,[CardID] GIFTCARDNUMBER" +
	  $" ,[TransactionDateTime] POSTRANSACTIONDATE" +
		$" ,[TransactionDateTime] TransactionDate" +
	  $" ,[Value] TRANSACTIONAMOUNT,[TransactionType],[StoreNo] StoreID,[StoreName],[PAN] " +
	  $"   ,[EAN],[StoreCommission] " +
	  $"   ,[IsActive],[IsDeleted],[CreatedAt],[CreatedBy],[UpdatedAt],[UpdatedBy], EPAYCardID " +
	  $" FROM [dbo].[BlackHawkSales] " +
	  $" where IsActive=1 and IsDeleted =0 " +
	  $"  order by TransactionDateTime desc");
			var Data = dapper.Query<BlackHawkSalesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public BlackHawkSalesEntity GetBlackHawkDataByID(string ID)
		{

			var result = GetBlackHawkSalesList().Where(x=>x.REFERENCENUMBER == ID).FirstOrDefault();
			return result;
		}
		public int DeleteBlackHawkInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[BlackHawkSales] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [BlackHawkSalesID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int TransformBlackHawkSalesData()
		{
			int result = 0;
			var listBHSales = GetBlackHawkSalesList();
			if(listBHSales !=null && listBHSales.Count > 0)
			{
				foreach (var item in listBHSales)
				{
					string Query = "";
					// Update Expiry Date Value
					Query = string.Format($" declare @vEPAYSalesID bigint " +
						$" if Exists(Select top 1 1 from BlackHawkSales where IsActive = 1 and IsDeleted = 0) " +
						$" begin " +
						$" Select top 1 @vEPAYSalesID = SalesID  " +
						$" from Sales where Cast(Date as date) = Cast('"+item.TransactionDate+"' as date) and Value = '"+item.TRANSACTIONAMOUNT+"' and IsActive = 1 and IsDeleted = 0  " +
						$" and(RetailerCode = 'BH' or ISNULL(RetailerCode, '') = '')  and StoreNo= 1  " +
						$" if (@vEPAYSalesID > 0)  " +
						$"	begin  " +
						$"	Update Sales  " +
						$"	set GGCCommission = (Select top 1 Commission from Retailer where RetailerCode = 'BH' and IsActive = 1 and IsDeleted = 0)  " +
						$"	,RetailerCode = 'BH', UpdatedAt = GetDate() " +
						$" where SalesID = @vEPAYSalesID " +
						$" Update  BlackHawkSales\r\nSet EPAYCardID = (Select top 1 CardID from Sales where SalesID= @vEPAYSalesID),UpdatedAt=GetDate()\r\n" +
						$" where BlackHawkSalesID = '"+item.BlackHawkSalesID+"' " +
						$" end " +
						$" end ");

					result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
				}
			}
			
			return result;

		}
		#endregion

	}
}
