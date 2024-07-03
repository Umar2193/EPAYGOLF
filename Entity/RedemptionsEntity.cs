using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class RedemptionsEntity
	{
		public Int64 RedemptionsID { get; set; }
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
		public DateTime? Date { get; set; }
		public decimal ProductCommission { get; set; }
		public decimal ProductAmount { get; set; }
		public decimal VATRate { get; set; }
		public decimal VATDueOnCommission { get; set; }
		public decimal AmountPayableToStore { get; set; }
		public string Postcode { get; set; }
		public DateTime? StatementCreated { get; set; }
		public string StatementNumber { get; set; }
		public decimal StatementAmount { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CreatedAt { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Int64 UpdatedBy { get; set; }
        public Int64 UserID { get; set; }
        public string Email { get; set; }
		public string UserEmail { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
	public class RedeptionImportDataEntity
	{
		public Int64 ID { get; set; }
		public string AccountName { get; set; }
		public string TransactionID { get; set; }
		public string TransactionType { get; set; }
		public string CardID { get; set; }
		public string PAN { get; set; }
		public string TransactionDateTime { get; set; }
		public decimal Value { get; set; }
		public string BINNumber { get; set; }
		public Int64 StoreNo { get; set; }
		public string StoreName { get; set; }
		public string Product { get; set; }
		public Int64 EAN { get; set; }


	}
	public class RedemptionsRemittance
	{
		public RedemptionsRemittance()
		{
			lstRedemptions = new List<RedemptionsEntity>();
		}
		public string StoreID { get; set; }
		public string StoreName { get; set; }
		public string DatePeriod { get; set; }
		public string InvoiceNumber { get; set; }
		public string DocumentDate { get; set; }
        public decimal GrossAmount { get; set; }
		public decimal ProductCommission { get; set; }

		public decimal VATDue { get; set; }

		public decimal AmountPayable { get; set; }
        public decimal InvoiceTotal { get; set; }
        public string baseURL { get; set; }
        public List<RedemptionsEntity> lstRedemptions { get; set; }
	}
}
