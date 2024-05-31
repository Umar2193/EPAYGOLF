using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.VATRates
{
	public class VATRatesRepository:IVATRatesRepository
	{
		DAL.DALVATRates _DALVATRates = new DAL.DALVATRates();
		public List<VATRatesEntity> GetVATRatesList()
		{
			try
			{

				return _DALVATRates.GetVATRatesList();
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public VATRatesEntity GetVATRatesDetail(Int64 ID)
		{
			try
			{

				return _DALVATRates.GetVATRatesDetail(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return null;
			}
		}
		public int SaveVATRatesInformation(VATRatesEntity obj)
		{
			try
			{

				return _DALVATRates.SaveVATRatesInformation(obj);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
		public int DeleteVATRatesInformation(Int64 ID)
		{
			try
			{

				return _DALVATRates.DeleteVATRatesInformation(ID);
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				return -1;
			}
		}
	}
}
