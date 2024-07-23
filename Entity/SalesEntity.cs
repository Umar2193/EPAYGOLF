using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class SalesEntity
	{
		public Int64 SalesID { get; set; }
		public Int64 ID { get; set; }
		public string AccountName { get; set; }
		public string TransactionID { get; set; }
		public string TransactionType { get; set; }
		public string CardID { get; set; }
		public string PAN { get; set; }
		public DateTime? TransactionDateTime { get; set; }
		public decimal Value { get; set; }
		public string BINNumber { get; set; }
		public Int64 StoreNo { get; set; }
		public string StoreName { get; set; }
		public string Product { get; set; }
		public Int64 EAN { get; set; }
		public decimal StoreCommission { get; set; }
		public decimal VATRate { get; set; }
		public DateTime? Date { get; set; }
		public decimal StoreAmount { get; set; }
		public decimal GGCCommission { get; set; }
		public decimal GGCAmount { get; set; }
		public decimal ProcessCommission { get; set; }
		public decimal ProcessAmount { get; set; }
		public decimal StripeCommission { get; set; }
		public decimal StripeAmount { get; set; }
		public decimal TransactionAmount { get; set; }
		public decimal NetAmount { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public decimal RedeemedAmount { get; set; }
		public decimal UnRedeemedAmount { get; set; }
		public decimal Breakage { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CreatedAt { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Int64 UpdatedBy { get; set; }
	}
	public class SalesImportDataEntity
	{
		public Int64 ID {  get; set; }
		public string AccountName { get; set; }
		public string TransactionID { get; set; }
		public string TransactionType { get; set; }
		public string CardID { get; set; }
		public string PAN { get; set; }
		public string TransactionDateTime { get; set; }
		public string Value { get; set; }
		public string BINNumber { get; set; }
		public Int64 StoreNo { get; set; }
		public string StoreName { get; set; }
		public string Product { get; set; }
		public Int64 EAN { get; set; }

	}
	public class MonthlySalesEntity
	{
        public string MonthYear { get; set; }
        public int RecordCount { get; set; }
        public decimal TotalValue { get; set; }
		public decimal TotalStoreAmount { get; set; }

		public decimal TotalProcessAmount { get; set; }

		public decimal TotalStripeAmount { get; set; }

		public decimal TotalTransactionAmount { get; set; }
		public decimal TotalNetAmount { get; set; }
		public decimal TotalRedeemedAmount { get; set; }
		public decimal TotalUnRedeemedAmount { get; set; }
		public decimal TotalGGCAmount { get; set; }
		public decimal TotalBreakage { get; set; }
		public DateTime? TransactionDateTime { get; set; }
	}
}
