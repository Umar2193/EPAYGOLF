using Entity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
	public class DALSales
	{
		DapperManager dapper;
		public DALSales()
		{
			dapper = new DapperManager();
		}
		public List<SalesEntity> GetSalesList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [SalesID],[ID],[AccountName],[TransactionID],[TransactionType] " +
	  $" , [CardID], [PAN], [TransactionDateTime], [Value], [BINNumber] " +
	  $" , [StoreNo], [StoreName], [Product], [EAN], isnull([StoreCommission],0.0) * 100 as StoreCommission " +
	  $" , isnull([VATRate],0.0) * 100 as VATRate, [Date], [StoreAmount], isnull([GGCCommission],0.0) * 100 as GGCCommission, [GGCAmount] " +
	  $" , isnull([ProcessCommission],0.0) * 100 as ProcessCommission, [ProcessAmount], isnull([StripeCommission],0.0) * 100 as StripeCommission, [StripeAmount], [TransactionAmount] " +
	  $" , [NetAmount], [ExpiryDate], [RedeemedAmount], [UnRedeemedAmount], [Breakage] " +
	  $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
	  $" FROM [dbo].[Sales] " +
	  $" where IsActive=1 and IsDeleted =0 " +
	  $"  order by TransactionDateTime desc");
			var Data = dapper.Query<SalesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public SalesEntity GetSalesDetail(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [SalesID],[ID],[AccountName],[TransactionID],[TransactionType] " +
	  $" , [CardID], [PAN], [TransactionDateTime], [Value], [BINNumber] " +
	  $" , [StoreNo], [StoreName], [Product], [EAN], isnull([StoreCommission],0.0) * 100 as StoreCommission " +
	  $" , isnull([VATRate],0.0) * 100 as VATRate, [Date], [StoreAmount], isnull([GGCCommission],0.0) * 100 as GGCCommission, [GGCAmount] " +
	  $" , isnull([ProcessCommission],0.0) * 100 as ProcessCommission, [ProcessAmount], isnull([StripeCommission],0.0) * 100 as StripeCommission, [StripeAmount], [TransactionAmount] " +
	  $" , [NetAmount], [ExpiryDate], [RedeemedAmount], [UnRedeemedAmount], [Breakage] " +
	  $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
	  $" FROM [dbo].[Sales] s" +
	  $" where IsActive=1 and IsDeleted =0 " +
				$" and s.SalesID = " + id);
			var Data = dapper.Query<SalesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList().FirstOrDefault();
		}
		public int SaveSalesInformation(SalesEntity obj)
		{
			string Query = "";
			if (obj.SalesID > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Sales] " +
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
					$",[StoreCommission] ='" + obj.StoreCommission + "'" +
					$",[VATRate] ='" + obj.VATRate + "'" +
					$",[Date] ='" + obj.Date + "'" +
					$",[StoreAmount] ='" + obj.StoreAmount + "'" +
					$",[GGCCommission] ='" + obj.GGCCommission + "'" +
					$",[GGCAmount] ='" + obj.GGCAmount + "'" +
					$",[ProcessCommission] ='" + obj.ProcessCommission + "'" +
					$",[ProcessAmount] ='" + obj.ProcessAmount + "'" +
					$",[StripeCommission] ='" + obj.StripeCommission + "'" +
					$",[StripeAmount] ='" + obj.StripeAmount + "'" +
					$",[TransactionAmount] ='" + obj.TransactionAmount + "'" +
					$",[NetAmount] ='" + obj.NetAmount + "'" +
					$",[ExpiryDate] ='" + obj.ExpiryDate + "'" +
					$",[RedeemedAmount] ='" + obj.RedeemedAmount + "'" +
					$",[UnRedeemedAmount] ='" + obj.UnRedeemedAmount + "'" +
					$",[Breakage] ='" + obj.Breakage + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [SalesID] = '" + obj.SalesID + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Sales] ([ID],[AccountName],[TransactionID],[TransactionType]\r\n      ,[CardID],[PAN],[TransactionDateTime],[Value],[BINNumber]\r\n      ,[StoreNo],[StoreName],[Product],[EAN],[StoreCommission]\r\n      ,[VATRate],[Date],[StoreAmount],[GGCCommission],[GGCAmount]\r\n ,[ProcessCommission] ,[ProcessAmount] ,[StripeCommission] ,[StripeAmount] ,[TransactionAmount]\r\n      ,[NetAmount]\r\n      ,[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy],[UpdatedBy] )" +
					$" VALUES('" + obj.ID + "','" + obj.AccountName + "'" +
					$",'" + obj.TransactionID + "' ,'" + obj.TransactionType + "'" +
					$",'" + obj.CardID + "' ,'" + obj.PAN + "'" +
					$",'" + obj.TransactionDateTime + "' ,'" + obj.Value + "'" +
					$",'" + obj.BINNumber + "' ,'" + obj.StoreNo + "'" +
					$",'" + obj.StoreName + "' ,'" + obj.Product + "'" +
					$",'" + obj.EAN + "' ,'" + obj.StoreCommission + "'" +
					$",'" + obj.VATRate + "' ,'" + obj.Date + "'" +
					$",'" + obj.StoreAmount + "' ,'" + obj.GGCCommission + "'" +
					$",'" + obj.GGCAmount + "' ,'" + obj.ProcessCommission + "'" +
					$",'" + obj.ProcessAmount + "' ,'" + obj.StripeCommission + "'" +
					$",'" + obj.StripeAmount + "' ,'" + obj.TransactionAmount + "'" +
					$",'" + obj.NetAmount + "'" +
					$",'1' ,'0'" +
					$", GetDate() ,GetDate()" +
					$",'100'" +
					$",'100' )");

			}
			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public int DeleteSalesInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Sales] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [SalesID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public SalesEntity GetSalesDataByID(string ID)
		{

			var result = GetSalesList().Where(x => x.ID == (Convert.ToInt64(ID))).FirstOrDefault();
			return result;
		}
		public int TransformSalesData()
		{
			string Query = "";
			// Update Expiry Date Value
			Query = "UPDATE Sales \r\nSET Sales.ExpiryDate =  DateAdd(YEAR, 1, Sales.Date) \r\nWHERE isnull(Sales.ExpiryDate,'')  = '' ";

			//Update Store Commission Value

			Query += "\r\n UPDATE Sales\r\nSET Sales.StoreCommission = Commissions.Commission \r\nfrom \r\nSales\r\nINNER JOIN Commissions ON Sales.StoreNo = Commissions.RetailerID \r\nWHERE (isnull(Sales.StoreCommission,0) = 0) ";

			//Update Store Commission Amount

			Query += "\r\n UPDATE Sales\r\nSET Sales.StoreAmount = (Sales.Value * -1)  * Sales.StoreCommission \r\nWHERE (isnull(Sales.StoreAmount,0) = 0) ";

			//Update GGC Commission Value
			Query += "\r\n UPDATE Sales\r\nSET Sales.GGCCommission = Commissions.GGCRedemption\r\nfrom Sales\r\nINNER JOIN Commissions ON Sales.StoreNo = Commissions.RetailerID \r\nWHERE (isnull(Sales.GGCCommission,0) =0) ";

			//Update GGC Commission Amount
			Query += "\r\n UPDATE Sales \r\nSET Sales.GGCAmount = Sales.Value * Sales.GGCCommission \r\nWHERE (isnull(Sales.GGCAmount,0) =0) ";

			//Update Processing Fee
			Query += "\r\n UPDATE Sales \r\nSET Sales.ProcessCommission = Commissions.ProcessingFee\r\nfrom Sales\r\nINNER JOIN Commissions ON\r\nSales.StoreNo = Commissions.RetailerID \r\nWHERE (isnull(Sales.ProcessCommission,0) = 0)";

			//Update Processing Amount
			Query += "\r\n UPDATE Sales\r\nSET Sales.ProcessAmount = (Sales.Value * -1) * Sales.ProcessCommission \r\nWHERE (isnull(Sales.ProcessAmount,0) = 0) ";

			//Update Stripe Commission
			Query += "\r\n UPDATE Sales\r\nSET Sales.StripeCommission = Commissions.StripeFee \r\nfrom Sales\r\nINNER JOIN Commissions ON \r\nSales.StoreNo = Commissions.RetailerID \r\nWHERE (ISNULL(Sales.StripeCommission,0) = 0) ";

			//Update Stripe Amount
			Query += "\r\n UPDATE Sales \r\nSET Sales.StripeAmount = (Sales.Value * -1) * Sales.StripeCommission\r\nWHERE (isnull(Sales.StripeAmount,0) = 0) ";

			//Update Transaction Fee Amount
			Query += "\r\n UPDATE Sales\r\nSET Sales.TransactionAmount = (Commissions.TransactionFee * -1)\r\nfrom Sales\r\nINNER JOIN Commissions ON \r\nSales.StoreNo = Commissions.RetailerID\r\nWHERE (isnull(Sales.TransactionAmount,0) =0) ";

			//Update Net Amount
			Query += "\r\n UPDATE Sales \r\nSET Sales.NetAmount = ROUND(Sales.GGCAmount + Sales.ProcessAmount + \r\nSales.StripeAmount + Sales.TransactionAmount + Sales.StoreAmount, 2) \r\nWHERE (isnull(Sales.NetAmount,0) = 0) ";

			//Update Redeemed Amount Value
			Query += "\r\n \r\nUPDATE Sales \r\nSET RedeemedAmount = (Select Sum(Value)  from Redemptions where CardID = Sales.CardID) ";

			//Update Redeemed Amount Value Where NULL (in order to calculate next query as NZ does nto work
			Query += "\r\n UPDATE Sales\r\nSET RedeemedAmount = 0 WHERE RedeemedAmount IS NULL ";

			//Update UnRedeemed Amount Value
			Query += "\r\n UPDATE Sales\r\nSET UnRedeemedAmount = Value + RedeemedAmount ";

			// Update UnRedeemed Amount Value Where NULL (in order to calculate next query as NZ does nto work
			Query += "\r\n UPDATE Sales \r\nSET UnRedeemedAmount = 0 WHERE UnRedeemedAmount IS NULL ";

			//Update Breakage Value
			Query += "\r\n UPDATE Sales\r\nSET Breakage = [Value] - [RedeemedAmount] \r\nWHERE Cast(ExpiryDate as date) <= Cast(GetDate() as date) AND ExpiryDate IS NOT NULL\r\nAND Breakage IS NULL";


			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;

		}
		public List<SalesEntity> GetSalesListReport(Int64 ProductID, Int64 SalesStoreNo, DateTime startDate, DateTime endDate)
		{

			string _Query = $"SELECT [SalesID],[ID],[AccountName],[TransactionID],[TransactionType] " +
	  $" , [CardID], [PAN], [TransactionDateTime], [Value], [BINNumber] " +
	  $" , [StoreNo], [StoreName], [Product], [EAN], isnull([StoreCommission],0.0) * 100 as StoreCommission " +
	  $" , isnull([VATRate],0.0) * 100 as VATRate, [Date], [StoreAmount], isnull([GGCCommission],0.0) * 100 as GGCCommission, [GGCAmount] " +
	  $" , isnull([ProcessCommission],0.0) * 100 as ProcessCommission, [ProcessAmount], isnull([StripeCommission],0.0) * 100 as StripeCommission, [StripeAmount], [TransactionAmount] " +
	  $" , [NetAmount], [ExpiryDate], [RedeemedAmount], [UnRedeemedAmount], [Breakage] " +
	  $" , [IsActive], [IsDeleted], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy] " +
	  $" FROM [dbo].[Sales] " +
	  $" where IsActive=1 and IsDeleted =0 ";

			//Product
			if (ProductID == 2) //All Golf Products
			{
				_Query = _Query + $" and [EAN] LIKE '5%'";
			}
			else if (ProductID == 3) //All Cycling Products
			{
				_Query = _Query + $" and [EAN] LIKE '6%'";
			}
			else if (ProductID == 4) //All Fishing Products
			{
				_Query = _Query + $" and [EAN] LIKE '7%'";
			}
			else
			{
				if (ProductID > 0)
				{
					_Query = _Query + $" and [EAN] = " + ProductID;
				}
			}

			// Sales Store
			if (SalesStoreNo > 0)
			{
				_Query = _Query + $" and [StoreNo] = " + SalesStoreNo;
			}


			//Date
			_Query = _Query + $" and cast([Date] as date) >= Cast('" + startDate + "' as date) ";
			_Query = _Query + $" and cast([Date] as date) <= Cast('" + endDate + "' as date) ";


			_Query = _Query + $"  order by TransactionDateTime desc ";
			string Query = string.Format(_Query);
			var Data = dapper.Query<SalesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public List<MonthlySalesEntity> GetSalesMonthlyListReport(Int64 ProductID, Int64 SalesStoreNo, DateTime startDate, DateTime endDate)
		{
			string _ProductClause = string.Empty;
			string _SalesStoreNoClause = string.Empty;
			//Product
			if (ProductID == 2) //All Golf Products
			{
				_ProductClause = $" and [EAN] LIKE '5%'";
			}
			else if (ProductID == 3) //All Cycling Products
			{
				_ProductClause = $" and [EAN] LIKE '6%'";
			}
			else if (ProductID == 4) //All Fishing Products
			{
				_ProductClause = $" and [EAN] LIKE '7%'";
			}
			else
			{
				if (ProductID > 0)
				{
					_ProductClause = $" and [EAN] = " + ProductID;
				}
			}

			// Sales Store
			if (SalesStoreNo > 0)
			{
				_SalesStoreNoClause = $" and [StoreNo] = " + SalesStoreNo;
			}


			string _Query = $" WITH AggregatedData AS ( SELECT  " +

		$" FORMAT([Date], 'MMM-yyyy') AS MonthYear, " +
		$" COUNT(*) AS RecordCount, " +
		$" SUM([Value]) AS TotalValue, " +
		$" Sum(StoreAmount) AS TotalStoreAmount, " +
		$" Sum(ProcessAmount) as TotalProcessAmount, " +
		$" Sum(StripeAmount) as TotalStripeAmount, " +
		$" Sum(TransactionAmount) as TotalTransactionAmount, " +
		$" Sum(NetAmount) as TotalNetAmount, " +
		$" Sum(RedeemedAmount) as TotalRedeemedAmount, " +
		$" Sum(UnRedeemedAmount) as TotalUnRedeemedAmount, " +
		$" Sum(GGCAmount) as TotalGGCAmount, " +
		$" Sum(Breakage) as TotalBreakage " +
		$" FROM [dbo].[Sales] " +
		$" where IsActive = 1 and IsDeleted = 0 " +
		$" and cast([Date] as date) >= Cast('" + startDate + "' as date) " +
		$" and cast([Date] as date) <= Cast('" + endDate + "' as date) ";
			if (!string.IsNullOrEmpty(_ProductClause))
			{
				_Query = _Query + _ProductClause;
			}
			if (!string.IsNullOrEmpty(_SalesStoreNoClause))
			{
				_Query = _Query + _SalesStoreNoClause;
			}

			_Query = _Query +
			$" GROUP BY FORMAT([Date], 'MMM-yyyy')) " +

			$" Select * from ( SELECT  " +
			$" ROW_NUMBER() OVER (PARTITION BY  FORMAT(t.Date, 'MMM-yyyy') order by FORMAT(t.Date, 'MMM-yyyy')) AS rn,  " +
			$" a.MonthYear,  " +
			$" a.RecordCount,  " +
			$" a.TotalValue,  " +
			$" a.TotalStoreAmount,  " +
			$" a.TotalProcessAmount,  " +
			$" a.TotalStripeAmount,  " +
			$" a.TotalTransactionAmount,  " +
			$" a.TotalNetAmount,  " +
			$" a.TotalRedeemedAmount,  " +
			$" a.TotalUnRedeemedAmount,  " +
			$" a.TotalGGCAmount,  " +
			$" a.TotalBreakage,  " +
			$" t.*  " +
			 $" FROM[dbo].[Sales]  t  " +
			$" JOIN  AggregatedData a ON FORMAT(t.Date, 'MMM-yyyy') = a.MonthYear  " +
			$" where IsActive = 1 and IsDeleted = 0  " +
			$" and cast([Date] as date) >= Cast('" + startDate + "' as date) " +
			$" and cast([Date] as date) <= Cast('" + endDate + "' as date) ";
			if (!string.IsNullOrEmpty(_ProductClause))
			{
				_Query = _Query + _ProductClause;
			}
			if (!string.IsNullOrEmpty(_SalesStoreNoClause))
			{
				_Query = _Query + _SalesStoreNoClause;
			}


			_Query = _Query + $" )p  " +
		$" where p.rn = 1  " +
		$" ORDER BY  p.TransactionDateTime  ";


			string Query = string.Format(_Query);
			var Data = dapper.Query<MonthlySalesEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
	}
}
