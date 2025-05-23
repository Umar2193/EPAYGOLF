﻿using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sales
{
	public interface ISalesRepository
	{
		List<SalesEntity> GetSalesList();
		SalesEntity GetSalesDetail(Int64 ID);
		int SaveSalesInformation(SalesEntity obj);
		int DeleteSalesInformation(Int64 ID);
		SalesEntity GetSalesDataByID(string ID);
		int TransformSalesData();
		List<SalesEntity> GetSalesListReport(Int64 ProductID, string SalesStoreNo, DateTime startDate, DateTime endDate,string retailercode="");
		List<MonthlySalesEntity> GetSalesMonthlyListReport(Int64 ProductID, string SalesStoreNo, DateTime startDate, DateTime endDate, string retailercode = "");
	}
}
