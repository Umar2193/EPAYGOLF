using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.StoresRedeem
{
	public interface IStoresRedeemRepository
	{
		List<StoresRedeemEntity> GetStoresRedeemList();
		StoresRedeemEntity GetStoresRedeemDetail(Int64 ID);
		int SaveStoresRedeemInformation(StoresRedeemEntity obj);
		int DeleteStoresRedeemInformation(Int64 ID);
	}
}
