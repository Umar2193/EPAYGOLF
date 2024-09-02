using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Retailer
{
	public interface IRetailerRepository
	{
		List<RetailerEntity> GetRetailerList();
        RetailerEntity GetRetailerDetail(Int64 ID);
		int SaveRetailerInformation(RetailerEntity obj);
		int DeleteRetailerInformation(Int64 ID);
		int SaveBlackHawkSalesInformation(BlackHawkSalesEntity obj);
		BlackHawkSalesEntity GetBlackHawkDataByID(string ID);
		List<BlackHawkSalesEntity> GetBlackHawkSalesList();
		int DeleteBlackHawkInformation(Int64 ID);
		int TransformBlackHawkSalesData();
	}
}
