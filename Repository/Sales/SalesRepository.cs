﻿using Entity;
using Repository.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sales
{
	public class SalesRepository : ISalesRepository
	{
		DAL.DALSales _DALsales = new DAL.DALSales();
		public List<SalesEntity> GetSalesList()
		{
			try
			{

				return _DALsales.GetSalesList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public SalesEntity GetSalesDetail(Int64 ID)
		{
			try
			{

				return _DALsales.GetSalesDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveSalesInformation(SalesEntity obj)
		{
			try
			{

				return _DALsales.SaveSalesInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteSalesInformation(Int64 ID)
		{
			try
			{

				return _DALsales.DeleteSalesInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public SalesEntity GetSalesDataByID(string ID)
		{
			try
			{

				return _DALsales.GetSalesDataByID(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int TransformSalesData()
		{
			try
			{

				return _DALsales.TransformSalesData();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
	}
}
