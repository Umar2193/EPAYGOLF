using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL
{
	public class DALRedemptions
	{
		DapperManager dapper;
		public DALRedemptions()
		{
			dapper = new DapperManager();
		}
		public List<RedemptionsEntity> GetRedemptionsList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT r.[RedemptionsID] ,r.[ID],r.[AccountName],r.[TransactionID] " +
	  $" , r.[TransactionType], r.[CardID], r.[PAN], r.[TransactionDateTime] " +
	  $" , r.[Value], r.[BINNumber], r.[StoreNo], r.[StoreName], r.[Product] " +
	  $" , r.[EAN], r.[Date], isnull(r.[ProductCommission],0.0) * 100 as ProductCommission, r.[ProductAmount],  isnull(r.[VATRate],0.0) * 100 as VATRate " +
	  $" , r.[VATDueOnCommission], r.[AmountPayableToStore], r.[StatementCreated] " +
	  $" , r.[StatementNumber], r.[StatementAmount], r.[IsActive], r.[IsDeleted] " +
	  $" , r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy] " +
	  $" , sr.[PostCode]  Postcode" +
	  $" FROM [dbo].[Redemptions]  r" +
	   $" left join [dbo].[StoresRedeem] sr on  sr.StoreNo = r.StoreNo" +
	  $" where r.IsActive = 1 and r.IsDeleted = 0 " +
	  $" order by r.TransactionDateTime desc");
			var Data = dapper.Query<RedemptionsEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public RedemptionsEntity GetRedemptionsDetail(Int64 id = 0)
		{

			string Query = string.Format($"SELECT r.[RedemptionsID], r.[ID], r.[AccountName], r.[TransactionID] " +
	  $" , r.[TransactionType], r.[CardID], r.[PAN], r.[TransactionDateTime] " +
	  $" , r.[Value], r.[BINNumber], r.[StoreNo], r.[StoreName], r.[Product] " +
	  $" , r.[EAN], r.[Date], isnull(r.[ProductCommission],0.0) * 100 as ProductCommission, r.[ProductAmount],  isnull(r.[VATRate],0.0) * 100 as VATRate " +
	  $" , r.[VATDueOnCommission], r.[AmountPayableToStore], r.[StatementCreated] " +
	  $" , r.[StatementNumber], r.[StatementAmount], r.[IsActive], r.[IsDeleted] " +
	  $" , r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy] " +
	  $" , sr.[PostCode]  Postcode" +
	  $" FROM [dbo].[Redemptions]  r" +
	   $" left join [dbo].[StoresRedeem] sr on  sr.StoreNo = r.StoreNo" +
	  $" where r.IsActive = 1 and r.IsDeleted = 0 " +
				$" and r.RedemptionsID = " + id);
			var Data = dapper.Query<RedemptionsEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveRedemptionsInformation(RedemptionsEntity obj)
		{
			string Query = "";
			if (obj.RedemptionsID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Redemptions] " +
					$"SET [ID] = '" + obj.ID + "'" +
					$",[AccountName] = '" + obj.AccountName + "' " +
					$",[TransactionID] = '" + obj.TransactionID + "'" +
					$",[TransactionType] ='" + obj.TransactionType + "'" +
					$",[CardID] ='" + obj.CardID + "'" +
					$",[PAN] ='" + obj.PAN + "'" +
					$",[TransactionDateTime] ='" + obj.TransactionDateTime + "'" +
					$",[Value] ='" + obj.Value + "'" +
					$",[BINNumber] ='" + obj.BINNumber + "'" +
					$",[StoreNo] ='" + obj.StoreNo + "'" +
					$",[StoreName] ='" + obj.StoreName + "'" +
					$",[Product] ='" + obj.Product + "'" +
					$",[EAN] ='" + obj.EAN + "'" +
					$",[Date] ='" + obj.Date + "'" +
					$",[ProductCommission] ='" + obj.ProductCommission + "'" +
					$",[ProductAmount] ='" + obj.ProductAmount + "'" +
					$",[VATRate] ='" + obj.VATRate + "'" +
					$",[VATDueOnCommission] ='" + obj.VATDueOnCommission + "'" +
					$",[AmountPayableToStore] ='" + obj.AmountPayableToStore + "'" +
					$",[Postcode] ='" + obj.Postcode + "'" +
					$",[StatementCreated] ='" + obj.StatementCreated + "'" +
					$",[StatementNumber] ='" + obj.StatementNumber + "'" +
					$",[StatementAmount] ='" + obj.StatementAmount + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [RedemptionsID] = '" + obj.RedemptionsID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Redemptions] ([ID],[AccountName],[TransactionID]\r\n      ,[TransactionType],[CardID],[PAN],[TransactionDateTime]\r\n      ,[Value],[BINNumber],[StoreNo],[StoreName],[Product]\r\n      ,[EAN],[Date],[ProductCommission],[ProductAmount],[VATRate]\r\n      ,[VATDueOnCommission],[AmountPayableToStore],[Postcode],[IsActive],[IsDeleted]\r\n      ,[CreatedAt],[UpdatedAt],[CreatedBy],[UpdatedBy])" +
					$" VALUES('" + obj.ID + "','" + obj.AccountName + "'" +
					$",'" + obj.TransactionID + "' ,'" + obj.TransactionType + "'" +
					$",'" + obj.CardID + "' ,'" + obj.PAN + "'" +
					$",'" + obj.TransactionDateTime + "' ,'" + obj.Value + "'" +
					$",'" + obj.BINNumber + "' ,'" + obj.StoreNo + "'" +
					$",'" + obj.StoreName + "' ,'" + obj.Product + "'" +
					$",'" + obj.EAN + "' ,'" + obj.Date + "'" +
					$",'" + obj.ProductCommission + "' ,'" + obj.ProductAmount + "'" +
					$",'" + obj.VATRate + "' ,'" + obj.VATDueOnCommission + "'" +
					$",'" + obj.AmountPayableToStore + "' ,'" + obj.Postcode + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteRedemptionsInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Redemptions] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [RedemptionsID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public RedemptionsEntity GetredeemDataByID(string ID)
		{

			var result = GetRedemptionsList().Where(x => x.ID == (Convert.ToInt64(ID))).FirstOrDefault();
			return result;
		}
		public int TransformRedeemData()
		{
			string Query = "";
			// Update Product Commission Value
			Query = "UPDATE Redemptions \r\nSet Redemptions.ProductCommission = Sales.GGCCommission \r\nfrom Redemptions\r\nINNER JOIN Sales ON Redemptions.CardID = Sales.CardID \r\nWHERE isnull(Redemptions.ProductCommission,0) =0 ";

			//Update Commission Value

			Query += "\r\n UPDATE Redemptions\r\nSet Redemptions.ProductAmount = Redemptions.Value * Redemptions.ProductCommission\r\nWHERE Redemptions.ProductCommission >0.0 AND Redemptions.Value is not null ";

			//Update VAT Rate Value

			Query += "\r\n UPDATE Redemptions \r\nSet Redemptions.VATRate = Sales.VATRate\r\nfrom Redemptions\r\nINNER JOIN Sales ON Redemptions.CardID = Sales.CardID \r\nWHERE Redemptions.VATRate Is Null ";

			//Update Commission VAT Value
			Query += "\r\n UPDATE Redemptions \r\nSet Redemptions.VATDueOnCommission = Redemptions.ProductAmount * Redemptions.VATRate\r\n WHERE Redemptions.ProductCommission > 0.0 AND Redemptions.Value IS NOT NULL ";

			//Update AmountPayableToStore
			Query += "\r\n UPDATE Redemptions\r\nSet Redemptions.AmountPayableToStore = Redemptions.Value - Redemptions.ProductAmount - Redemptions.VATDueOnCommission\r\nWHERE Redemptions.ProductCommission > 0.0 AND Redemptions.Value is not null ";

			//Update Postcode - 3 steps
			Query += "\r\n UPDATE Redemptions \r\nSet Redemptions.Postcode = StoresRedeem.Postcode\r\nfrom Redemptions\r\nINNER JOIN StoresRedeem ON Redemptions.StoreNo = StoresRedeem.StoreNo\r\n WHERE Redemptions.Postcode IS NULL ";

			//Update Redeemed Amount Value
			Query += "\r\n \r\nUPDATE Sales \r\nSET RedeemedAmount = (Select Sum(Value)  from Redemptions where CardID = Sales.CardID) ";

			//Update Redeemed Amount Value Where NULL (in order to calculate next query as NZ does nto work
			Query += "\r\n UPDATE Sales\r\nSET RedeemedAmount = 0 WHERE RedeemedAmount IS NULL ";


			//Update Breakage Value
			Query += "\r\n UPDATE Sales\r\nSET Breakage = [Value] - [RedeemedAmount] \r\nWHERE Cast(ExpiryDate as date) <= Cast(GetDate() as date) AND ExpiryDate IS NOT NULL\r\nAND Breakage IS NULL";


			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;

		}
		public List<RedemptionsEntity> GetRedemptionsListReport(Int64 ProductID, Int64 redemStoreNo, DateTime startDate, DateTime endDate,Int64 UserID= 0)
		{

			string _Query = $"SELECT r.[RedemptionsID] ,r.[ID],r.[AccountName],r.[TransactionID] " +
	  $" , r.[TransactionType], r.[CardID], r.[PAN], r.[TransactionDateTime] " +
	  $" , r.[Value], r.[BINNumber], r.[StoreNo], r.[StoreName], r.[Product] " +
	  $" , r.[EAN], r.[Date], isnull(r.[ProductCommission],0.0) * 100 as ProductCommission, r.[ProductAmount],  isnull(r.[VATRate],0.0) * 100 as VATRate " +
	  $" , r.[VATDueOnCommission], r.[AmountPayableToStore], r.[StatementCreated] " +
	  $" , r.[StatementNumber], r.[StatementAmount], r.[IsActive], r.[IsDeleted] " +
	  $" , r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy] " +
	  $" , sr.[PostCode]  Postcode" +
	   $" , sr.[UserID]  UserID " +
	   $" , sr.[Email]  Email " +
	   $" , sr.[UserEmail]  UserEmail " +
		 $" , sr.[Title]  Title " +
		   $" , sr.[FirstName]  FirstName " +
			 $" , sr.[LastName]  LastName " +
	  $" FROM [dbo].[Redemptions]  r" +
	   $" left join [dbo].[StoresRedeem] sr on  sr.StoreNo = r.StoreNo" +
	  $" where r.IsActive = 1 and r.IsDeleted = 0 ";

			//Product
			if (ProductID == 2) //All Golf Products
			{
				_Query = _Query + $" and r.[EAN] LIKE '5%'";
			}
			else if (ProductID == 3) //All Cycling Products
			{
				_Query = _Query + $" and r.[EAN] LIKE '6%'";
			}
			else if (ProductID == 4) //All Fishing Products
			{
				_Query = _Query + $" and r.[EAN] LIKE '7%'";
			}
			else
			{
				if (ProductID > 0)
				{
					_Query = _Query + $" and r.[EAN] = " + ProductID;
				}
			}

			// Sales Store
			if (redemStoreNo > 0 && UserID ==0)
			{
				_Query = _Query + $" and r.[StoreNo] = " + redemStoreNo;
			}
			if(UserID > 0)
			{
				_Query = _Query + $" and sr.[UserID] = " + UserID;
			}

			//Date
			_Query = _Query + $" and cast(r.[Date] as date) >= Cast('" + startDate + "' as date) ";
			_Query = _Query + $" and cast(r.[Date] as date) <= Cast('" + endDate + "' as date) ";


			_Query = _Query + $"  order by r.TransactionDateTime desc ";
			string Query = string.Format(_Query);
			var Data = dapper.Query<RedemptionsEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public List<InvoiceEntity> GetInvoiceList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [ID],[InvoiceNumber],[StoreNo],[StatementCreated],[StatementNumber],[GrossAmount] " +

		$" , [ProductCommission], [VATDue], [AmountPayable] " +

		$" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +

		$"  FROM[dbo].[Invoices]" +
	    $" where IsActive = 1 and IsDeleted = 0 " +
	    $" order by StatementCreated desc");
			var Data = dapper.Query<InvoiceEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public int SaveInvoiceInformation(InvoiceEntity obj)
		{
			string Query = "";
			if (obj.ID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Invoices] " +
					$"SET [StoreNo] = '" + obj.StoreNo + "'" +
					$",[StatementCreated] = '" + obj.StatementCreated + "' " +
					$",[StatementNumber] = '" + obj.StatementNumber + "'" +
					$",[GrossAmount] ='" + obj.GrossAmount + "'" +
					$",[ProductCommission] ='" + obj.ProductCommission + "'" +
					$",[VATDue] ='" + obj.VATDue + "'" +
					$",[AmountPayable] ='" + obj.AmountPayable + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [InvoiceNumber] = '" + obj.InvoiceNumber + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Invoices] ([InvoiceNumber],[StoreNo],[StatementCreated],[StatementNumber],[GrossAmount],[ProductCommission],[VATDue],[AmountPayable],[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy],[UpdatedBy])" +
					$" VALUES('" + obj.InvoiceNumber + "','" + obj.StoreNo + "'" +
					$", GetDate() ,'" + obj.StatementNumber + "'" +
					$",'" + obj.GrossAmount + "' ,'" + obj.ProductCommission + "'" +
					$",'" + obj.VATDue + "' ,'" + obj.AmountPayable + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int UpdateRedemptionsInvoiceInformation(RedemptionsEntity obj)
		{
			string Query = "";
			if (obj.RedemptionsID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Redemptions] " +
					$" set [StatementCreated] = GetDate()" +
					$",[StatementNumber] ='" + obj.StatementNumber + "'" +
					$",[StatementAmount] ='" + obj.StatementAmount + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [RedemptionsID] = '" + obj.RedemptionsID + "'");
			}
			
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
	}
}
