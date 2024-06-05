using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SalesStore
{
	public interface ISalesStoreRepository
	{
		List<SalesStoreEntity> GetSalesStoreList();
		SalesStoreEntity GetSalesStoreDetail(Int64 ID);
		int SaveSalesStoreInformation(SalesStoreEntity obj);
		int DeleteSalesStoreInformation(Int64 ID);
		List<SalesStoreEntity> GetSalesStoreByStoreNo(string storeno);
	}
}
