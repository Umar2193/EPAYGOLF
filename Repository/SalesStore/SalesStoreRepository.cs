using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SalesStore
{
	public class SalesStoreRepository :ISalesStoreRepository
	{
		DAL.DALSalesStore _DALSalesStore = new DAL.DALSalesStore();
		public List<SalesStoreEntity> GetSalesStoreList()
		{
			try
			{

				return _DALSalesStore.GetSalesStoreList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public SalesStoreEntity GetSalesStoreDetail(Int64 ID)
		{
			try
			{

				return _DALSalesStore.GetSalesStoreDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveSalesStoreInformation(SalesStoreEntity obj)
		{
			try
			{

				return _DALSalesStore.SaveSalesStoreInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteSalesStoreInformation(Int64 ID)
		{
			try
			{

				return _DALSalesStore.DeleteSalesStoreInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public List<SalesStoreEntity> GetSalesStoreByStoreNo(string storeno)
		{
			try
			{

				return _DALSalesStore.GetSalesStoreByStoreNo(storeno);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}

	}
}
