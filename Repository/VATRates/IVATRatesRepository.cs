using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.VATRates
{
	public interface IVATRatesRepository
	{
		List<VATRatesEntity> GetVATRatesList();
		VATRatesEntity GetVATRatesDetail(Int64 ID);
		int SaveVATRatesInformation(VATRatesEntity obj);
		int DeleteVATRatesInformation(Int64 ID);
	}
}
