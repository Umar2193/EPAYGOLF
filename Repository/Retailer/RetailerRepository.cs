using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Retailer
{
	public class RetailerRepository:IRetailerRepository
	{
		DAL.DALRetailer _DALLayer = new DAL.DALRetailer();
		public List<RetailerEntity> GetRetailerList()
		{
			try
			{

				return _DALLayer.GetRetailerList();
			}
			catch(Exception ex) 
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public RetailerEntity GetRetailerDetail(Int64 ID)
		{
			try
			{

				return _DALLayer.GetRetailerDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveRetailerInformation(RetailerEntity obj)
		{
			try
			{

				return _DALLayer.SaveRetailerInformation(obj);
			}
			catch( Exception ex) 
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteRetailerInformation(Int64 ID)
		{
			try
			{

				return _DALLayer.DeleteRetailerInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		#region BlackHawkSales
		public int SaveBlackHawkSalesInformation(BlackHawkSalesEntity obj)
		{
			try
			{

				return _DALLayer.SaveBlackHawkSalesInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public BlackHawkSalesEntity GetBlackHawkDataByID(string ID)
		{
			try
			{

				return _DALLayer.GetBlackHawkDataByID(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public List<BlackHawkSalesEntity> GetBlackHawkSalesList()
		{
			try
			{

				return _DALLayer.GetBlackHawkSalesList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int DeleteBlackHawkInformation(Int64 ID)
		{
			try
			{

				return _DALLayer.DeleteBlackHawkInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int TransformBlackHawkSalesData()
		{
			try
			{

				return _DALLayer.TransformBlackHawkSalesData();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		#endregion

	}
}
