using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

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
	  $" , r.[Value], r.[BINNumber], r.[StoreNo], sr.[StoreName], r.[Product] " +
	  $" , r.[EAN], r.[Date], isnull(r.[ProductCommission],0.0) * 100 as ProductCommission, r.[ProductAmount],  isnull(r.[VATRate],0.0) * 100 as VATRate " +
	  $" , r.[VATDueOnCommission], r.[AmountPayableToStore], r.[StatementCreated] " +
	  $" , r.[StatementNumber], r.[StatementAmount], r.[IsActive], r.[IsDeleted] " +
	  $" , r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy] ,r.RemainingRedemptionAmount " +
	  $" , sr.[PostCode]  Postcode" +
	  $" ,sr.[SortCode],sr.[AccountNumber],sr.[NameBankAccountHolder] " +
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
	  $" , r.[Value], r.[BINNumber], r.[StoreNo], sr.[StoreName], r.[Product] " +
	  $" , r.[EAN], r.[Date], isnull(r.[ProductCommission],0.0) * 100 as ProductCommission, r.[ProductAmount],  isnull(r.[VATRate],0.0) * 100 as VATRate " +
	  $" , r.[VATDueOnCommission], r.[AmountPayableToStore], r.[StatementCreated] " +
	  $" , r.[StatementNumber], r.[StatementAmount], r.[IsActive], r.[IsDeleted] " +
	  $" , r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy], r.RemainingRedemptionAmount  " +
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
			if (!string.IsNullOrEmpty(obj.StoreName))
			{
				obj.StoreName = obj.StoreName.Replace("'", "''");
			}
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
			Query += "\r\n UPDATE Sales\r\nSET Breakage = [Value] + [RedeemedAmount] \r\nWHERE Cast(ExpiryDate as date) <= Cast(GetDate() as date) AND ExpiryDate IS NOT NULL\r\nAND Breakage IS NULL AND CAST(TransactionDateTime AS DATE) <= CAST(GETDATE() AS DATE)";

			Query += "   -- Drop temporary tables if they exist\r\n    IF OBJECT_ID('tempdb..#TempCumulativeRedemptions') IS NOT NULL DROP TABLE #TempCumulativeRedemptions;\r\n    IF OBJECT_ID('tempdb..#TempInitialValues') IS NOT NULL DROP TABLE #TempInitialValues;\r\n    IF OBJECT_ID('tempdb..#TempRemainingValues') IS NOT NULL DROP TABLE #TempRemainingValues;\r\n\r\n    -- Create a temporary table to store initial card values\r\n    CREATE TABLE #TempInitialValues (\r\n        CardID BIGINT,\r\n        InitialValue FLOAT\r\n    );\r\n\r\n    -- Populate the temporary table with initial card values from the Sales table\r\n    INSERT INTO #TempInitialValues (CardID, InitialValue)\r\n    SELECT CardID, Value\r\n    FROM Sales;\r\n\r\n    -- Create a temporary table to store cumulative redemption amounts\r\n    CREATE TABLE #TempCumulativeRedemptions (\r\n        CardID BIGINT,\r\n        RedemptionID BIGINT,\r\n        CumulativeRedemptionAmount FLOAT\r\n    );\r\n\r\n    -- Populate the temporary table with cumulative redemption amounts\r\n    INSERT INTO #TempCumulativeRedemptions (CardID, RedemptionID, CumulativeRedemptionAmount)\r\n    SELECT R.CardID, R.ID, SUM(R2.Value) AS CumulativeRedemptionAmount\r\n    FROM Redemptions AS R\r\n    INNER JOIN Redemptions AS R2 ON R.CardID = R2.CardID AND R2.TransactionDateTime <= R.TransactionDateTime AND R2.ID <= R.ID\r\n    GROUP BY R.CardID, R.ID;\r\n\r\n    -- Create a temporary table to store remaining redemption amounts\r\n    CREATE TABLE #TempRemainingValues (\r\n        CardID BIGINT,\r\n        RedemptionID BIGINT,\r\n        RemainingRedemptionAmount FLOAT\r\n    );\r\n\r\n    -- Populate the temporary table with remaining redemption amounts\r\n    INSERT INTO #TempRemainingValues (CardID, RedemptionID, RemainingRedemptionAmount)\r\n    SELECT C.CardID, C.RedemptionID, (I.InitialValue + C.CumulativeRedemptionAmount) AS RemainingRedemptionAmount\r\n    FROM #TempCumulativeRedemptions AS C\r\n    INNER JOIN #TempInitialValues AS I ON C.CardID = I.CardID;\r\n\r\n    -- Update the original table using the temporary table\r\n    UPDATE R\r\n    SET R.RemainingRedemptionAmount = T.RemainingRedemptionAmount\r\n    FROM Redemptions AS R\r\n    INNER JOIN #TempRemainingValues AS T ON R.CardID = T.CardID AND R.ID = T.RedemptionID;\r\n\r\n    -- Drop the temporary tables\r\n    DROP TABLE #TempCumulativeRedemptions;\r\n    DROP TABLE #TempInitialValues;\r\n    DROP TABLE #TempRemainingValues;";

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;

		}
		public List<RedemptionsEntity> GetRedemptionsListReport(Int64 ProductID, string redemStoreNo, DateTime startDate, DateTime endDate, Int64 UserID = 0)
		{

			string _Query = $"SELECT r.[RedemptionsID] ,r.[ID],r.[AccountName],r.[TransactionID] " +
	  $" , r.[TransactionType], r.[CardID], r.[PAN], r.[TransactionDateTime] " +
	  $" , r.[Value], r.[BINNumber], r.[StoreNo], sr.[StoreName], r.[Product] " +
	  $" , r.[EAN], r.[Date], isnull(r.[ProductCommission],0.0) * 100 as ProductCommission, r.[ProductAmount],  isnull(r.[VATRate],0.0) * 100 as VATRate " +
	  $" , r.[VATDueOnCommission], r.[AmountPayableToStore], r.[StatementCreated] " +
	  $" , r.[StatementNumber], r.[StatementAmount], r.[IsActive], r.[IsDeleted] " +
	  $" , r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy] , r.RemainingRedemptionAmount " +
	  $" , sr.[PostCode]  Postcode" +
	   $" , sr.[UserID]  UserID " +
	   $" , sr.[Email]  Email " +
	   $" , sr.[UserEmail]  UserEmail " +
		 $" , sr.[Title]  Title " +
		   $" , sr.[FirstName]  FirstName " +
			 $" , sr.[LastName]  LastName " +
			   $" ,sr.[SortCode],sr.[AccountNumber],sr.[NameBankAccountHolder] " +
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
			if (!string.IsNullOrEmpty(redemStoreNo) && redemStoreNo!= "0" && UserID == 0)
			{
				_Query = _Query + $" and r.[StoreNo]= '" + redemStoreNo + "' ";
			}
			if (UserID > 0)
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

			string Query = string.Format($"SELECT r.[ID],r.[InvoiceNumber],r.[StoreNo],r.[StatementCreated],r.[StatementNumber],r.[GrossAmount] " +

		$" , r.[ProductCommission], r.[VATDue], r.[AmountPayable] " +

		$" , r.[IsActive], r.[IsDeleted], r.[CreatedAt], r.[CreatedBy], r.[UpdatedAt], r.[UpdatedBy] " +
		$" , r.document_url " +
		$" , r.DatePeriod " +
		$", sr.StoreName " +
		$" , sr.[UserID]  UserID " +
		$" , sr.[Email]  EmailTo " +
		$" , sr.[UserEmail]  UserEmail " +
		$" , sr.[Title]  Title " +
		$" , sr.[FirstName] + ' ' + sr.[LastName]  as UserName " +


		$"  FROM [dbo].[Invoices] r" +
		$" left join [dbo].[StoresRedeem] sr on  sr.StoreNo = r.StoreNo " +
		$" where r.IsActive = 1 and r.IsDeleted = 0 " +
		$" order by r.InvoiceNumber desc");
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
					$",[DatePeriod] ='" + obj.DatePeriod + "'" +
					$",[UserId] ='" + obj.UserId + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [InvoiceNumber] = '" + obj.InvoiceNumber + "'");
			}
			else
			{
				Query = string.Format($"INSERT INTO [dbo].[Invoices] ([InvoiceNumber],[StoreNo],[StatementCreated],[StatementNumber],[GrossAmount],[ProductCommission],[VATDue],[AmountPayable],[DatePeriod],UserId,[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy],[UpdatedBy])" +
					$" VALUES('" + obj.InvoiceNumber + "','" + obj.StoreNo + "'" +
					$", GetDate() ,'" + obj.StatementNumber + "'" +
					$",'" + obj.GrossAmount + "' ,'" + obj.ProductCommission + "'" +
					$",'" + obj.VATDue + "' ,'" + obj.AmountPayable + "'  ,'" + obj.DatePeriod + "'  ,'" + obj.UserId + "'" +
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
		public int UpdateInvoiceDocument(InvoiceEntity obj)
		{
			string Query = "";
			if (obj.InvoiceNumber > 0)
			{
				Query = string.Format($"UPDATE [dbo].[Invoices] " +
					$"SET [document_url] = '" + obj.document_url + "'" +
					$",[UpdatedAt] = GetDate() " +
					$",[UpdatedBy] = 100 " +
					$" WHERE [InvoiceNumber] = '" + obj.InvoiceNumber + "'");
			}

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		#region BreakageData
		public int SaveBreakageInformation(BreakageRedeemEntity obj)
		{
			string Query = "";
			string trndateQuery = obj.TransactionDate.ToString();
			string expdateQuery =  obj.ExpiryDate.ToString();
			if(!string.IsNullOrEmpty(expdateQuery))
			{
				expdateQuery = string.Format($"'"+ expdateQuery + "'");
			}
			else 
			{
				expdateQuery = string.Format("NULL");
			}
			if (!string.IsNullOrEmpty(trndateQuery))
			{
				trndateQuery = string.Format($"'" + trndateQuery + "'");
			}
			else
			{
				trndateQuery = string.Format("NULL");
			}

			Query = string.Format($"INSERT INTO [dbo].[Breakage] ([Activation_TXID] ,[gift_card_number] ,[gift_card_id],[category],[initial_face_value]" +
				$",[redemption_amount],[current_value]" +
				$",[TransactionDate],[ExpiryDate],[gift_card_status],[redemption_status],[redemption_month],[redemption_year],[Totally_Direct]" +
				$" ,[IsActive],[IsDeleted],[CreatedAt],[UpdatedAt],[CreatedBy],[UpdatedBy] )" +
				$" VALUES('" + obj.Activation_TXID + "','" + obj.gift_card_number + "'" +
				$",'" + obj.gift_card_id + "' ,'" + obj.category + "'" +
				$",'" + obj.initial_face_value + "' ,'" + obj.redemption_amount + "'" +
				$",'" + obj.current_value + "' ," + trndateQuery + "" +
				$","+ expdateQuery + " ,'" + obj.gift_card_status + "'" +
				$",'" + obj.redemption_status + "' ,'" + obj.redemption_month + "'" +
				$",'" + obj.redemption_year + "' ,'" + obj.Totally_Direct + "'" +
				$",'1' ,'0'" +
				$", GetDate() ,GetDate()" +
				$",'100'" +
				$",'100' )");


			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		public List<BreakageRedeemEntity> GetBreakageDataList(Int64 id = 0)
		{

			string Query = string.Format($"SELECT [BreakageID] ,[Activation_TXID] ,[gift_card_number] ,[MerchantTransactionID] " +
	  $" ,[gift_card_id],[category],[initial_face_value],[redemption_amount],[current_value]" +
	  $" ,[TransactionDate],[ExpiryDate] " +
	  $" ,[gift_card_status],[redemption_status],[redemption_month],[redemption_year],[Totally_Direct] " +
	  $" ,[IsActive],[IsDeleted],[CreatedAt],[CreatedBy],[UpdatedAt],[UpdatedBy] " +
	  $" FROM [dbo].[Breakage] " +
	  $" where IsActive=1 and IsDeleted =0 " +
	  $"  order by CreatedAt desc");
			var Data = dapper.Query<BreakageRedeemEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		public BreakageRedeemEntity GetBreakageDataByID(string ID)
		{

			var result = GetBreakageDataList().Where(x => x.Activation_TXID == ID).FirstOrDefault();
			return result;
		}
		public int DeletBreakageInformation(Int64 ID)
		{
			string Query = "";

			Query = string.Format($"UPDATE [dbo].[Breakage] SET [IsActive] = '0'" +
				$",[IsDeleted] = '1' " +
				$",[UpdatedAt] = GetDate() " +
				$",[UpdatedBy] = 100 " +
				$" WHERE [BreakageID] = '" + ID + "'");

			var result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
			return result;
		}
		//public int TransformBlackHawkSalesData()
		//{
		//	int result = 0;
		//	var listBHSales = GetBlackHawkSalesList();
		//	if (listBHSales != null && listBHSales.Count > 0)
		//	{
		//		foreach (var item in listBHSales)
		//		{
		//			string Query = "";
		//			// Update Expiry Date Value
		//			Query = string.Format($" declare @vEPAYSalesID bigint " +
		//				$" if Exists(Select top 1 1 from BlackHawkSales where IsActive = 1 and IsDeleted = 0) " +
		//				$" begin " +
		//				$" Select top 1 @vEPAYSalesID = SalesID  " +
		//				$" from Sales where Cast(Date as date) = Cast('" + item.TransactionDate + "' as date) and Value = '" + item.TRANSACTIONAMOUNT + "' and IsActive = 1 and IsDeleted = 0  " +
		//				$" and(RetailerCode = 'BH' or ISNULL(RetailerCode, '') = '')  and StoreNo= 1  " +
		//				$" if (@vEPAYSalesID > 0)  " +
		//				$"	begin  " +
		//				$"	Update Sales  " +
		//				$"	set GGCCommission = (Select top 1 Commission from Retailer where RetailerCode = 'BH' and IsActive = 1 and IsDeleted = 0)  " +
		//				$"	,RetailerCode = 'BH', UpdatedAt = GetDate() " +
		//				$" where SalesID = @vEPAYSalesID " +
		//				$" Update  BlackHawkSales\r\nSet EPAYCardID = (Select top 1 CardID from Sales where SalesID= @vEPAYSalesID),UpdatedAt=GetDate()\r\n" +
		//				$" where BlackHawkSalesID = '" + item.BlackHawkSalesID + "' " +
		//				$" end " +
		//				$" end ");

		//			result = dapper.Execute<int>(Query, null, null, true, null, CommandType.Text);
		//		}
		//	}

		//	return result;

		//}
		public List<BreakageRedeemEntity> GetBreakageListReport(Int64 ProductID, string redemStoreNo, DateTime startDate, DateTime endDate, Int64 UserID = 0)
		{

			string _Query = $"SELECT [BreakageID] ,[Activation_TXID] ,[gift_card_number] ,[MerchantTransactionID] " +
	  $" ,[gift_card_id],[category],[initial_face_value],[redemption_amount],[current_value]" +
	  $" ,[TransactionDate],[ExpiryDate] " +
	  $" ,[gift_card_status],[redemption_status],[redemption_month],[redemption_year],[Totally_Direct] " +
	  $" ,[IsActive],[IsDeleted],[CreatedAt],[CreatedBy],[UpdatedAt],[UpdatedBy] " +
	  $" FROM [dbo].[Breakage] r " +
	  $" where r.IsActive = 1 and r.IsDeleted = 0 and r.[TransactionDate] is not null";

			////Product
			//if (ProductID == 2) //All Golf Products
			//{
			//	_Query = _Query + $" and r.[EAN] LIKE '5%'";
			//}
			//else if (ProductID == 3) //All Cycling Products
			//{
			//	_Query = _Query + $" and r.[EAN] LIKE '6%'";
			//}
			//else if (ProductID == 4) //All Fishing Products
			//{
			//	_Query = _Query + $" and r.[EAN] LIKE '7%'";
			//}
			//else
			//{
			//	if (ProductID > 0)
			//	{
			//		_Query = _Query + $" and r.[EAN] = " + ProductID;
			//	}
			//}

			//// Sales Store
			//if (redemStoreNo > 0 && UserID == 0)
			//{
			//	_Query = _Query + $" and r.[StoreNo] = " + redemStoreNo;
			//}
			//if (UserID > 0)
			//{
			//	_Query = _Query + $" and sr.[UserID] = " + UserID;
			//}

			//Date
			_Query = _Query + $" and cast(r.[ExpiryDate] as date) >= Cast('" + startDate + "' as date) ";
			_Query = _Query + $" and cast(r.[ExpiryDate] as date) <= Cast('" + endDate + "' as date) ";


			_Query = _Query + $"  order by r.CreatedAt desc ";
			string Query = string.Format(_Query);
			var Data = dapper.Query<BreakageRedeemEntity>(Query, null, null, true, null, CommandType.Text);
			return Data.ToList();
		}
		#endregion
	}
}
