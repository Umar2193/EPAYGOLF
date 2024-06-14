using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Redemptions
{
	public interface IRedemptionsRepository
	{
		List<RedemptionsEntity> GetRedemptionsList();
		RedemptionsEntity GetRedemptionsDetail(Int64 ID);
		int SaveRedemptionsInformation(RedemptionsEntity obj);
		int DeleteRedemptionsInformation(Int64 ID);
		RedemptionsEntity GetredeemDataByID(string ID);
		int TransformRedeemData();
		List<RedemptionsEntity> GetRedemptionsListReport(Int64 ProductID, Int64 redemStoreNo, DateTime startDate, DateTime endDate);
	}
}
