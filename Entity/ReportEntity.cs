using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class ReportEntity
	{
	}
	public class ReportSearchParam
	{
        public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
        public string reportPeriod  { get; set; }
        public Int64 salesstoreno { get; set; }
		public string salesstorename { get; set; }
		public Int64 redemstoreno { get; set; }
		public string redemstorename { get; set; }
		public Int64 productid { get; set; }
        public string  productName { get; set; }

    }

	public class ReportSummaryModel
	{
        public string TotalValue { get; set; }
        public string StoreAmount { get; set; }
		public string ProcessAmount { get; set; }

		public string StripeAmount { get; set; }
		public string TransactionAmount { get; set; }
		public string NetAmount { get; set; }
		public string RedeemedAmount { get; set; }
		public string UnRedeemedAmount { get; set; }
        public int SalesTotalCount { get; set; }
		public int RedeemTotalCount { get; set; }
        public string ggcBreakage { get; set; }
    }

}
