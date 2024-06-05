using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.StoresRedeem
{
	public class StoresRedeemRepository: IStoresRedeemRepository
	{
		DAL.DALStoresRedeem _DALStoresRedeem = new DAL.DALStoresRedeem();
		public List<StoresRedeemEntity> GetStoresRedeemList()
		{
			try
			{

				return _DALStoresRedeem.GetStoresRedeemList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public StoresRedeemEntity GetStoresRedeemDetail(Int64 ID)
		{
			try
			{

				return _DALStoresRedeem.GetStoresRedeemDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveStoresRedeemInformation(StoresRedeemEntity obj)
		{
			try
			{

				return _DALStoresRedeem.SaveStoresRedeemInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteStoresRedeemInformation(Int64 ID)
		{
			try
			{

				return _DALStoresRedeem.DeleteStoresRedeemInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public List<StoresRedeemEntity> GetStoreRedeemByStoreNo(string storeno)
		{
			try
			{

				return _DALStoresRedeem.GetStoreRedeemByStoreNo(storeno);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
	}
}
