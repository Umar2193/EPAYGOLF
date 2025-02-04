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
		List<RedemptionsEntity> GetRedemptionsListReport(Int64 ProductID, string redemStoreNo, DateTime startDate, DateTime endDate,Int64 UserID=0);
		List<InvoiceEntity> GetInvoiceList(Int64 id = 0);
		int SaveInvoiceInformation(InvoiceEntity obj);
		int UpdateRedemptionsInvoiceInformation(RedemptionsEntity obj);
		int UpdateInvoiceDocument(InvoiceEntity obj);
		int SaveBreakageInformation(BreakageRedeemEntity obj);
		BreakageRedeemEntity GetBreakageDataByID(string ID);
		List<BreakageRedeemEntity> GetBreakageListReport(Int64 ProductID, string redemStoreNo, DateTime startDate, DateTime endDate, Int64 UserID = 0);
	}
}
