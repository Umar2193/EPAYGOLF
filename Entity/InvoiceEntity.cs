using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	public class InvoiceEntity
	{
        public Int64 ID { get; set; }
        public Int64 InvoiceNumber { get; set; }
        public string StoreNo { get; set; }
        public DateTime StatementCreated { get; set; }
        public string StatementNumber { get; set; }
        public decimal GrossAmount { get; set; }
		public decimal ProductCommission { get; set; }
		public decimal VATDue { get; set; }
		public decimal AmountPayable { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CreatedAt { get; set; }
		public Int64 CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Int64 UpdatedBy { get; set; }
        public  string document_url { get; set; }
        public string StoreName { get; set; }
        public string DatePeriod { get; set; }
        public Int64 UserId { get; set; }
		public string UserName { get; set; }
        public string EmailTo { get; set; }

    }
}
